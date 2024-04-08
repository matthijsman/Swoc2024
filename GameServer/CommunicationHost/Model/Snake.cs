using CommunicationHost.Utilities;

namespace CommunicationHost.Model
{
    public class Snake
    {
        public int Length { get; set; } = 1;

        public Snake(string name)
        {
            Name = name;
        }

        public Snake(LinkedList<Cell> segments, string name)
        {
            Length = segments.Count;
            Segments = segments;
            Name = name;
        }

        public long GetScore()
        {
            return (long)Math.Pow(Length, 4);
        }

        public void Eat()
        {
            Length++;
            Console.WriteLine($"snake {Name} is now {Length} long");
        }

        public LinkedList<Cell> Segments { get; set; } = new LinkedList<Cell>();
        public string Name { get; }

        public int[] GetHead()
        {
            return Segments.Last.Value.Address;
        }

        public int[]? Move(Cell nextPos)
        {
            int[]? res = null;
            if(Segments.Count >= Length)
            {
                res = Segments.FirstOrDefault()?.Address;
                Segments.RemoveFirst();
            }
            Segments.AddLast(nextPos);
            return res;
        }

        public Snake Split(int segment, string name)
        {
            var newSnakeCells = new LinkedList<Cell>();
            for (int i = 0; i < segment; i++)
            {
                var cell = Segments.First;
                Segments.RemoveFirst();
                newSnakeCells.AddLast(cell);
            }
            Length = Segments.Count;
            return new Snake(newSnakeCells, name);
        }
    }
}