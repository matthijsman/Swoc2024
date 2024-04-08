
using System.Runtime.InteropServices;
using CommunicationHost.GameEngine;
using CommunicationHost.Model;
using PlayerInterface;
using Grpc.Core;
using CommunicationHost.Utilities;

namespace CommunicationHost.Services
{
    public class PlayerService : PlayerHost.PlayerHostBase
    {
        private readonly ILogger<PlayerService> _logger;
        private static SemaphoreSlim _playerLock = new SemaphoreSlim(1);
        private readonly BattleSnake _gameEngine;
        private static List<BasicUpdateReceiver<GameUpdateMessage>> _players = new List<BasicUpdateReceiver<GameUpdateMessage>>();
        private static List<BasicUpdateReceiver<ServerUpdateMessage>> _stateListeners = new List<BasicUpdateReceiver<ServerUpdateMessage>>();

        public PlayerService(ILogger<PlayerService> logger, BattleSnake gameEngine)
        {
            _logger = logger;
            _gameEngine = gameEngine;
        }

        public override async Task SubscribeToServerEvents(EmptyRequest request, IServerStreamWriter<ServerUpdateMessage> responseStream, ServerCallContext context)
        {
            var newListener = new BasicUpdateReceiver<ServerUpdateMessage>();
            newListener.AddStream(responseStream);
            try
            {
                await _playerLock.WaitAsync();
                
                foreach (var player in _players.Where(p => p is Player))
                {
                    await responseStream.WriteAsync(new ServerUpdateMessage
                    {
                        MessageType = MessageType.PlayerJoined,
                        Message = player.Name

                    });
                }
            }
            finally
            {
                _playerLock.Release();
            }

            lock (_stateListeners)
            {
                _stateListeners.Add(newListener);
            }
            await newListener.WaitForMessages();
        }

        public override async Task<GameSettings> Register(RegisterRequest request, ServerCallContext context)
        {
            var settings = _gameEngine.GetGameSettings();

            if (!string.IsNullOrWhiteSpace(request.PlayerName))
            {
                var playerIdentifier = Guid.NewGuid().ToString();
                var newPlayer = new Player(request.PlayerName, playerIdentifier, context.CancellationToken);
                await _playerLock.WaitAsync();
                _players.Add(newPlayer);
                Console.WriteLine($"new player: {request.PlayerName}: {playerIdentifier}");
                _playerLock.Release();
                PlayerJoined(request.PlayerName);

                int[] startAddress = _gameEngine.RegisterPlayer(newPlayer);
                settings.StartAddress.AddRange(startAddress);
                settings.PlayerIdentifier = playerIdentifier;
            }

            return settings;
        }

        public override Task Subscribe(SubsribeRequest request, IServerStreamWriter<GameUpdateMessage> responseStream, ServerCallContext context)
        {
            BasicUpdateReceiver<GameUpdateMessage> player;
            if (string.IsNullOrWhiteSpace(request.PlayerIdentifier))
            {
                var name = Guid.NewGuid().ToString();
                player = new BasicUpdateReceiver<GameUpdateMessage>(name, name, context.CancellationToken);
                _playerLock.Wait();
                _players.Add(player);
                _playerLock.Release();
            }
            else
            {
                _playerLock.Wait();
                player = _players.FirstOrDefault(p => p.Identifier == request.PlayerIdentifier);
                _playerLock.Release();
            }
            if (player != null)
            {
                player.AddStream(responseStream);
                return player.WaitForMessages();
            }

            return Task.CompletedTask;
        }

        public override Task<EmptyRequest> MakeMove(Move request, ServerCallContext context)
        {
            _gameEngine.RegisterPlayerMove(request).FireAndForget();
            return Task.FromResult(new EmptyRequest());
        }

        public override Task<EmptyRequest> SplitSnake(SplitRequest request, ServerCallContext context)
        {
            _gameEngine.RegisterSnakeSplit(request).FireAndForget();
            return Task.FromResult(new EmptyRequest());
        }

        public override Task<GameStateMessage> GetGameState(EmptyRequest request, ServerCallContext context)
        {
            var message = _gameEngine.GetGameStateMessage();
            return Task.FromResult(message);
        }

        public static List<string> SendUpdates(GameUpdateMessage message)
        {
            var playersToRemove = new List<string>();
            try
            {
                _playerLock.Wait();
                foreach (var player in _players)
                {
                    var success = true;
                    success = player.SendUpdate(message);
                    if (!success && player.Name != player.Identifier)
                    {
                        Console.WriteLine($"removing player: {player.Identifier}");
                        playersToRemove.Add(player.Identifier);
                    }
                }
                _players.Where(x => playersToRemove.Contains(x.Identifier)).ToList().ForEach(x => x.StopPlayer());
                _players.RemoveAll(x => playersToRemove.Contains(x.Identifier));
            }
            finally
            {
                _playerLock.Release();
            }
            return playersToRemove;
        }


        private static void PlayerJoined(string playerName)
        {
            var serverMessage = new ServerUpdateMessage
            {
                MessageType = MessageType.PlayerJoined,
                Message = playerName
            };
            SendServerUpdate(serverMessage);
        }

        public static void StartGame()
        {
            var serverMessage = new ServerUpdateMessage
            {
                MessageType = MessageType.GameStateChange,
                Message = "Started!"
            };
            SendServerUpdate(serverMessage);
        }


        public static void EndGame()
        {

            var serverMessage = new ServerUpdateMessage
            {
                MessageType = MessageType.GameStateChange,
                Message = "Stopped!"
            };
            SendServerUpdate(serverMessage);

            try
            {
                _playerLock.Wait();
                foreach (var player in _players)
                {
                    if(player == null) continue;
                    player.StopPlayer();
                }
                _players.Clear();
            }
            finally 
            { 
                _playerLock.Release(); 
            }
            
        }

        private static void SendServerUpdate(ServerUpdateMessage message)
        {
            lock (_stateListeners)
            {
                foreach (var listener in _stateListeners)
                {
                    listener.SendUpdate(message);
                }
            }
        }
    }
}