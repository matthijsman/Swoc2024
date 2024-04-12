using CommunicationHost.Model;
using CommunicationHost.Services;
using CommunicationHost.Utilities;
using Newtonsoft.Json;
using PlayerInterface;
using System.Diagnostics;
using System.Diagnostics.Metrics;
using System.Text.Json.Serialization;
using static System.Formats.Asn1.AsnWriter;

namespace CommunicationHost.GameEngine
{
    public partial class BattleSnake
    {
        private Map _map;
        private SemaphoreSlim _playerWaiter = new SemaphoreSlim(1);
        private List<Player> _players = new List<Player>();
        private int[] _sideLengths;
        private bool _gameRunning;
        private Tick _tick;
        private SemaphoreSlim _moveLock = new SemaphoreSlim(1);

        public async Task Initialise(int[] sideLengths)
        {
            _players.Clear();
            _playerWaiter = new SemaphoreSlim(1);
            _map = new Map(sideLengths);
            _sideLengths = sideLengths;
            _playerWaiter.Wait();
            _tick = new Tick(_map);
            await RunGameLoop();
        }

        public void EndGame()
        {
            _gameRunning = false;
        }


        public int[] RegisterPlayer(Player player)
        {
            _players.Add(player);
            var maxRetries = 100;
            var tries = 0;
            int[] startAddress = _map.RandomAddress();
            var cell = _map.GetCell(startAddress);
            while(tries < maxRetries)
            {
                if(cell.Food?.Value == 1 || !string.IsNullOrWhiteSpace(cell.Player?.Name))
                {
                    tries++;
                    continue;
                }
                startAddress = _map.RandomAddress();
                cell = _map.GetCell(startAddress);
                break;
            }
            player.HomeBase = cell;
            player.SpawnSnake(player.Name);
            return startAddress;
        }

        public GameStateMessage GetGameStateMessage()
        {
            var updates = _map.GetCurrentGamestate();
            var message = new GameStateMessage();
            message.UpdatedCells.AddRange(updates);
            return message;
        }

        public GameSettings GetGameSettings()
        {
            var settings = new GameSettings
            {
                GameStarted = _gameRunning
            };
            settings.Dimensions.AddRange(_sideLengths);
            return settings;
        }

        public void StartGame()
        {
            _playerWaiter.Release();
        }

        public async Task RunGameLoop()
        {
            var nrFoods = 400;
            var updatedCells = new List<UpdatedCell>();
            for (int i = 0; i < nrFoods; i++)
            {
                var address = _map.RandomAddress();
                var cell = _map.GetCell(address);
                if (!string.IsNullOrWhiteSpace(cell.Player?.Name))
                {
                    continue;
                }
                cell.Food = new Food() { Value = 1 };
                var updatedCell = new UpdatedCell();
                updatedCell.Address.AddRange(address);
                updatedCell.FoodValue = 1;
                updatedCells.Add(updatedCell);
            }

            //wait for all players to connect
            await _playerWaiter.WaitAsync();
            _gameRunning = true;

            PlayerService.StartGame();

            var initialUpdateMessage = new GameUpdateMessage();
            initialUpdateMessage.PlayerScores.AddRange(_players.Select(x => new PlayerScore
            {
                PlayerName = x.Name,
                Score = 0
            }).ToList());
            initialUpdateMessage.UpdatedCells.AddRange(updatedCells);
            _tick = new Tick(_map);

            var disconnectedPlayers = SendUpdates(initialUpdateMessage);
            
            GameUpdateMessage message = null;
            while (_gameRunning)
            {
                await Task.Delay(1000);
                await DoLocked(async () => await _tick.ProcessMoves(_players, disconnectedPlayers));
                message = _tick.GetMessage();
                _tick = new Tick(_map);
                disconnectedPlayers = SendUpdates(message);
                var remainingFoods = _map.GetRemainingFoods();
                Console.WriteLine($"Number of food remaining: {remainingFoods}");
                if (remainingFoods == 0)
                {
                    _gameRunning = false;
                }
            }
            StorePlayerScores(message?.PlayerScores.ToList() ?? new List<PlayerScore>());
            PlayerService.EndGame();
        }
        
        private void StorePlayerScores(List<PlayerScore> scores)
        {
            var f1Scores = new int[] { 25, 18, 15, 12, 10, 8, 6, 4, 2, 1 };
            var fileName = "RunningScores.json";
            var currentScores = new List<PlayerScore>();
            if(File.Exists(fileName))
            {
                var curJson = File.ReadAllText(fileName);
                if(!string.IsNullOrEmpty(curJson))
                {
                    currentScores = JsonConvert.DeserializeObject<List<PlayerScore>>(curJson);
                }
            }
            currentScores ??= new List<PlayerScore>();
            scores = scores.OrderByDescending(x => x.Score).ToList();
            for(int i = 0; i< scores.Count; i++)
            {
                var playerScore = 0;
                if (i < 10)
                {
                    playerScore = f1Scores[i];
                }
                var player = currentScores.FirstOrDefault(x => x.PlayerName == scores[i].PlayerName);
                if(player != null)
                {
                    player.Score += playerScore;
                }
                else
                {
                    currentScores.Add(new PlayerScore
                    {
                        PlayerName = scores[i].PlayerName,
                        Score = playerScore,
                    });
                }
            }
            var str = JsonConvert.SerializeObject(currentScores);
            File.WriteAllText(fileName, str);
            foreach(var score in currentScores.OrderByDescending(x => x.Score)) 
            {
                Console.WriteLine($"Player: {score.PlayerName}, score: {score.Score}");
            }
        }

        private List<string> SendUpdates(GameUpdateMessage message)
        {
            return PlayerService.SendUpdates(message);
        }


        internal async Task RegisterPlayerMove(Move move)
        {
            await DoLocked(() => _tick.RegisterPlayerMove(move));
        }


        internal async Task RegisterSnakeSplit(SplitRequest split)
        {
           await DoLocked( () => _tick.RegisterPlayerSplit(split));
        }

        private async Task DoLocked(Action act)
        {
            await _moveLock.WaitAsync();
            try
            {
                act();
            }
            finally
            {
                _moveLock.Release();
            }

        }
    }
}
