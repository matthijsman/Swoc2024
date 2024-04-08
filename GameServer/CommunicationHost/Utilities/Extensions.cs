namespace CommunicationHost.Utilities
{
    public static class Extensions
    {
        public static void FireAndForget(this Task task)
        {
            Task.Run(() => task);
        }
        public static string Write(this int[] addr)
        {
            return string.Join(",", addr);
        }
    }
}
