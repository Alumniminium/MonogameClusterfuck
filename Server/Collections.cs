using System.Collections.Concurrent;

namespace Server
{
    public static class Collections
    {
        public static ConcurrentDictionary<uint, Player> Players = new ConcurrentDictionary<uint, Player>();
    }
}