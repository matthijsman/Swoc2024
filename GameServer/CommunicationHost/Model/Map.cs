using PlayerInterface;

namespace CommunicationHost.Model
{
    public class Map
    {
        public Array MapArray;
        public readonly int[] SideLengths;

        public Map(int[] sideLengths)
        {
            MapArray = Array.CreateInstance(typeof(Cell), sideLengths);
            LoopTings(MapArray, sideLengths, new int[sideLengths.Length], 0, InitCell);
            SideLengths = sideLengths;
        }

        public List<UpdatedCell> GetCurrentGamestate()
        {
            var updates = new List<UpdatedCell>();
            void getUpdate(Array array, int[] index) 
            {
                var cell = array.GetValue(index) as Cell;
                if (cell != null && ((cell.Food != null && cell.Food.Value > 0) || cell.Player != null))
                {
                    var update = new UpdatedCell
                    {
                        FoodValue = cell.Food?.Value ?? 0,
                        Player = cell.Player?.Name ?? ""
                    };
                    update.Address.AddRange(index);
                    updates.Add(update);
                }
            }

            LoopTings(MapArray, SideLengths, new int[SideLengths.Length], 0, getUpdate);

            return updates;
        }

        public int GetRemainingFoods()
        {
            var nrFoods = 0;
            void countFoods(Array array, int[] index)
            {
                var cell = array.GetValue(index) as Cell;
                if(cell != null && cell.Food!= null && cell.Food.Value > 0)
                {
                    nrFoods++;
                }
            }
            LoopTings(MapArray, SideLengths, new int[SideLengths.Length], 0, countFoods);
            return nrFoods;
        }

        private void LoopTings(Array array, int[] lengths, int[] curIndex, int dimension, Action<Array, int[]> func)
        {
            for (int i = 0; i < lengths[dimension]; i++)
            {
                curIndex[dimension] = i;
                if (dimension == array.Rank - 1)
                {
                    func(array, curIndex);
                }
                else
                {
                    LoopTings(array, lengths, curIndex, dimension + 1, func);
                }
            }
        }

        private static void InitCell(Array array, int[] curIndex)
        {
            int[] copyArray = new int[curIndex.Length];
            curIndex.CopyTo(copyArray, 0);
            array.SetValue(new Cell(copyArray), curIndex);
        }

        public Cell GetCell(int[] address)
        {
            var cell = MapArray.GetValue(address);
            if (cell == null)
            {
                throw new Exception($"Invalid address: {string.Join(",", address)}");
            }
            return (Cell)cell;
        }

        public int[] RandomAddress()
        {
            Random random = new Random();
            var address = new int[SideLengths.Length];

            for (int i = 0; i < SideLengths.Length; i++)
            {
                address[i] = random.Next(0, SideLengths[i]);
            }

            return address;
        }

        public void SetPlayer(Player player, int[] address)
        {
            var cell = GetCell(address);
            cell.Player = player;
        }

        public void SetFood(Food food, int[] address)
        {
            GetCell(address).Food = food;
        }
    }
}
