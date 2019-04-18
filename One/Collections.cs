using System.Collections.Concurrent;
using MonoGameClusterFuck.Entities;

namespace MonoGameClusterFuck
{
    public static class Collections
    {
        public static ConcurrentDictionary<uint, Entity> Entities = new ConcurrentDictionary<uint, Entity>();
    }
}
