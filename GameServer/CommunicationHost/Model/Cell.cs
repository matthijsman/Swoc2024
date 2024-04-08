namespace CommunicationHost.Model
{
    public class Cell
    {
        public int[] Address { get; set; }
        public Player? Player { get; set; }
        public Food? Food { get; set; }
        public bool Occupied => Player != null;

        public Cell(int dimensions)
        {
            Address = new int[dimensions];
        }
        public Cell(int[] address)
        {
            Address = address;
        }
    }
}
