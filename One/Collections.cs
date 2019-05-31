using System.Collections.Concurrent;
using One.Entities;

namespace One
{
    public static class Collections
    {
        public static ConcurrentDictionary<uint, Entity> Entities = new ConcurrentDictionary<uint, Entity>();
    }
}
