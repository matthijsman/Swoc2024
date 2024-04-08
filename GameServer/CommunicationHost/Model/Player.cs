using PlayerInterface;
using Grpc.Core;
using System.Xml.Linq;

namespace CommunicationHost.Model
{
    public class Player : BasicUpdateReceiver<GameUpdateMessage>
    {
        public Cell HomeBase;
        public List<Snake> Snakes = new List<Snake>();
        private long _savedSnakeScore;

        public Player(string name, string identifier, CancellationToken token) : base(name, identifier, token)
        {
        }

        public void SpawnSnake(string name)
        {
            var snake = new Snake(name);
            snake.Move(HomeBase);
            Snakes.Add(snake);
        }

        public Snake GetSnake(string name)
        {
            lock (Snakes)
            {
               return Snakes.FirstOrDefault(s => s.Name == name);
            }
        }

        public long GetScore()
        {
            return _savedSnakeScore + Snakes.Sum(s => s.GetScore());
        }

        public string GetSnakeForLocation(int[] address)
        {
            return Snakes.FirstOrDefault(s => s.Segments.Any(c => c.Address.SequenceEqual(address)))?.Name;
        }

        public List<int[]> RemoveSnake(string name, bool isSave = false)
        {
            Console.WriteLine($"removing snake {name} of player {Name}");
            var result = new List<int[]>();
            lock (Snakes)
            {
                var snake = Snakes.FirstOrDefault(s => s.Name == name);
                if (snake == null) return result;
                result.AddRange(snake.Segments.Select(s => s.Address));
                if (isSave)
                {
                    _savedSnakeScore += snake.GetScore();
                }
                Snakes.Remove(snake);
            }
            return result;
        }

        public List<int[]> RemoveAllSnakes()
        {
            var result = new List<int[]>();
            lock (Snakes)
            {
                foreach (var snake in Snakes)
                {
                    Console.WriteLine($"removing snake {snake.Name} of player {Name}");
                    result.AddRange(snake.Segments.Select(s => s.Address));
                }
                Snakes.Clear();
            }
            
            return result;
        }

        public void SplitSnake(string snakeToSplit, string newName, int snakeCell)
        {
            var oldSnake = Snakes.FirstOrDefault(s => s.Name == snakeToSplit);
            if (oldSnake != null)
            {
                lock (Snakes)
                {
                    Snakes.Add(oldSnake.Split(snakeCell, newName));
                }
            }
        }
    }
}