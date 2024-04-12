using CommunicationHost.Utilities;
using PlayerInterface;
using System.Xml.Linq;

namespace CommunicationHost.Model
{
    public class Tick
    {
        private GameUpdateMessage _updateMessage { get; set; }
        private List<Move> _moves { get; set; }
        private List<SplitRequest> _splits { get; set; }
        private readonly Map _map;

        public Tick(Map map)
        {
            _moves = new List<Move>();
            _splits = new List<SplitRequest>();
            _updateMessage = new GameUpdateMessage();
            _map = map;
        }

        public GameUpdateMessage GetMessage()
        {
            return _updateMessage;
        }

        public async Task<GameUpdateMessage> ProcessMoves(List<Player> players, List<string> disconnectedPlayers)
        {
            await Console.Out.WriteLineAsync($"processing {_moves.Count} moves");
            HandleDisconnectedPlayers(players, disconnectedPlayers);
            HandleSplits(players);
            HandleIllegalMoves(players);
            HandleHomeComing(players);
            HandleCollisions(players);
            HandleMoves(players);
            HandlePlayerScores(players);
            await Console.Out.WriteLineAsync($"Current Tick results in {_updateMessage.UpdatedCells.Count} updates");
            return _updateMessage;
        }

        public void RegisterPlayerMove(Move move)
        {
            _moves.Add(move);
        }

        public void RegisterPlayerSplit(SplitRequest split) 
        {
            _splits.Add(split);
        }

        private void HandleDisconnectedPlayers(List<Player> players, List<string> disconnectedPlayers)
        {
            foreach (var player in players.Where(p => disconnectedPlayers.Contains(p.Identifier)))
            {
                foreach(var snake in player.Snakes)
                {
                    _updateMessage.RemovedSnakes.Add($"{player.Name}:{snake.Name}");
                }
                var emptyAddresses = player.RemoveAllSnakes();
                emptyAddresses.ForEach(x => _map.SetPlayer(null, x));
                _updateMessage.UpdatedCells.AddRange(emptyAddresses.Select(x => GetUpdatedCell(x, 0, "")));

            }
            players.RemoveAll(p => disconnectedPlayers.Contains(p.Identifier));
        }

        private void HandleSplits(List<Player> players)
        {
            foreach (var split in _splits)
            {
                var move = new Move
                {
                    PlayerIdentifier = split.PlayerIdentifier,
                    SnakeName = split.NewSnakeName
                };
                move.NextLocation.AddRange(split.NextLocation);

                var player = players.FirstOrDefault(p => p.Identifier == split.PlayerIdentifier);
                if (player == null)
                {
                    Console.WriteLine($"Unable to process moves for player with Id: {split.PlayerIdentifier}, no such player exists");
                    continue;
                }
                player.SplitSnake(split.OldSnakeName, split.NewSnakeName, split.SnakeSegment);
                _moves.Add(move);
            }
        }

        private void HandleIllegalMoves(List<Player> players)
        {
            var removes = new List<Move>();
            foreach (var moveGroup in _moves.GroupBy(m => m.PlayerIdentifier))
            {
                var remSnakes = new List<Snake>();
                var player = players.FirstOrDefault(p => p.Identifier == moveGroup.Key);
                if (player == null)
                {
                    Console.WriteLine($"Unable to process moves for player with Id: {moveGroup.Key}, no such player exists");
                    continue;
                }
                foreach (var snake in player.Snakes)
                {
                    var numMoves = moveGroup.Count(m => m.SnakeName == snake.Name);
                    if(numMoves == 0)
                    {
                        Console.WriteLine($"No move for snake {snake.Name} of player {player.Name}");
                        continue;
                    }
                    else if (numMoves > 1)
                    {
                        Console.Error.WriteLine($"removing snake {snake.Name} of player {moveGroup.Key} because there was more than 1 move for it");
                        remSnakes.Add(snake);
                        removes.AddRange(moveGroup.Where(m => m.SnakeName == snake.Name));
                    }
                    else
                    {
                        var snakeMove = moveGroup.Single(m => m.SnakeName == snake.Name);
                        var snakeHeadLocation = snake.GetHead();
                        if (!IsLegalMove(snakeHeadLocation, snakeMove.NextLocation.ToArray()))
                        {
                            Console.Error.WriteLine($"removing snake {snake.Name} of player {moveGroup.Key} because {snakeHeadLocation.Write()} is not next to {snakeMove.NextLocation.ToArray().Write()}");
                            remSnakes.Add(snake);
                            removes.Add(snakeMove);
                        }
                    }
                }
                foreach (var snake in remSnakes)
                {
                    HandleSnakeRemove(player, snake.Name, false);
                }
            }
            removes.ForEach(m => _moves.Remove(m));
        }

        private void HandleHomeComing(List<Player> players)
        {
            var removes = new List<Move>();
            foreach (var moveGroup in _moves.GroupBy(x => x.PlayerIdentifier))
            {
                var player = players.FirstOrDefault(p => p.Identifier == moveGroup.Key);
                if(player == null)
                {
                    Console.WriteLine($"Unable to process moves for player with Id: {moveGroup.Key}, no such player exists");
                    continue;
                }
                var playerHome = player.HomeBase;
                var saveMoves = moveGroup.Where(m => m.NextLocation.SequenceEqual(playerHome.Address)).ToList();
                foreach (var move in saveMoves)
                {
                    Console.WriteLine($"{player.Name} saved snake {move.SnakeName}");
                    HandleSnakeRemove(player, move.SnakeName, true);
                    removes.Add(move);
                }
            }

            removes.ForEach(r => _moves.Remove(r));
        }

        private void HandleCollisions(List<Player> players)
        {
            var addressComparer = new AddressComparer();
            var removes = new List<Move>();
            foreach (var moveGroup in _moves.GroupBy(x => x.NextLocation.ToArray(), addressComparer))
            {
                if (moveGroup.Count() > 1)
                {
                    Console.WriteLine($"snakes collided: {string.Join(", ", moveGroup.Select(x => x.SnakeName))}");
                    foreach (var move in moveGroup)
                    {
                        var player = players.FirstOrDefault(p => p.Identifier == move.PlayerIdentifier);
                        HandleSnakeRemove(player, move.SnakeName, false);
                    }
                    removes.AddRange(moveGroup);
                    continue;
                }
            }
            foreach (var move in _moves)
            {
                var nextLocation = move.NextLocation.ToArray();
                var hitPlayer = _map.GetCell(nextLocation).Player;

                if(hitPlayer == null) continue;

                var hitSnake = hitPlayer.GetSnakeForLocation(nextLocation);
                var moveForHitSnake = _moves.FirstOrDefault(m => m.PlayerIdentifier == hitPlayer.Identifier && move.SnakeName == hitSnake);
                if (moveForHitSnake != null)
                {
                    removes.Add(moveForHitSnake);
                }

                HandleSnakeRemove(hitPlayer, hitSnake, false);
                var player = players.FirstOrDefault(p => p.Identifier == move.PlayerIdentifier);
                HandleSnakeRemove(player, move.SnakeName, false);
                
                Console.Error.WriteLine($"snake {player.Name}:{move.SnakeName} ran into {hitPlayer.Name}:{hitSnake} on location {nextLocation.Write()}");
                removes.Add(move);
            }

            removes.ForEach(r => _moves.Remove(r));
        }

        private void HandleMoves(List<Player> players)
        {
            foreach (var move in _moves)
            {
                var player = players.FirstOrDefault(p => p.Identifier == move.PlayerIdentifier);
                if (player == null)
                {
                    Console.WriteLine($"Unable to process moves for player with Id: {move.PlayerIdentifier}, no such player exists");
                    continue;
                }
                var snake = player.GetSnake(move.SnakeName);
                if (snake == null)
                {
                    Console.WriteLine($"Unable to process moves for snake {move.SnakeName} of player {player.Name}, no such snake exists");
                    continue;
                }
                var newCell = _map.GetCell(move.NextLocation.ToArray());
                if (newCell.Food != null)
                {
                    snake.Eat();
                    newCell.Food = null;
                }
                newCell.Player = player;
                var removedAddress = snake.Move(newCell);
                if (removedAddress != null)
                {
                    _updateMessage.UpdatedCells.Add(GetUpdatedCell(removedAddress, 0, ""));
                    _map.SetPlayer(null, removedAddress);
                }
                _updateMessage.UpdatedCells.Add(GetUpdatedCell(newCell.Address, 0, player.Name));
            }
            _moves.Clear();
        }

        private void HandlePlayerScores(List<Player> players)
        {
            foreach (var player in players)
            {
                _updateMessage.PlayerScores.Add(new PlayerScore
                {
                    PlayerName = player.Name,
                    Score = player.GetScore(),
                    Snakes = player.Snakes.Count
                });
            }
        }

        private void HandleSnakeRemove(Player player, string snakeName, bool isSave)
        {
            _updateMessage.UpdatedCells.AddRange(player.RemoveSnake(snakeName, isSave).Select(x => GetUpdatedCell(x, 0, "")));
            _updateMessage.RemovedSnakes.Add($"{player.Name}:{snakeName}");
        }

        private bool IsLegalMove(int[] from, int[] to)
        {
            if(!IsValidAddress(to)) return false;   
            var steps = 0;
            for (int i = 0; i < from.Length; i++)
            {
                steps += Math.Abs(from[i] - to[i]);
                if (steps > 1) return false;
            }
            return steps == 1;
        }

        private bool IsValidAddress(int[] addr)
        {
            if (addr.Length != _map.SideLengths.Length) return false;
            for (int i = 0; i < addr.Length; i++)
            {
                if (addr[i] < 0) return false;
                if (addr[i] >= _map.SideLengths[i]) return false;
            }
            return true;
        }

        private UpdatedCell GetUpdatedCell(int[] address, int food, string player)
        {
            var cell = new UpdatedCell();
            cell.Address.AddRange(address);
            cell.FoodValue = food;
            cell.Player = player;
            return cell;
        }
    }

    public static class TickExtentions
    {
        //public Tick
    }
}
