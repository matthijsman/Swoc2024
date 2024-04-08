using System.Diagnostics.CodeAnalysis;

namespace CommunicationHost.Utilities
{
    public class AddressComparer : IEqualityComparer<int[]>
    {
        public bool Equals(int[]? x, int[]? y)
        {
            return x.SequenceEqual(y);
        }

        public int GetHashCode([DisallowNull] int[] obj)
        {
            return obj.GetHashCode();
        }
    }
}
