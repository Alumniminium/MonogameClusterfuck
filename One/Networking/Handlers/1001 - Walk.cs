using MonoGameClusterFuck.Entities;
using MonoGameClusterFuck.Networking.Packets;
using MonoGameClusterFuck.SceneManagement;
using MonoGameClusterFuck.Systems;

namespace MonoGameClusterFuck.Networking.Handlers
{
    public static class Walk
    {
        public static void Handle(Player player, MsgWalk packet)
        {
            var uniqueId = packet.UniqueId;
            var location = packet.Location;
            var tickCount = packet.TickCount;
            Entity entity = null;

            if (uniqueId == player.UniqueId)
                return;

            if (Collections.Entities.TryGetValue(uniqueId, out entity))
            {
                ThreadedConsole.WriteLine("[Net][MsgWalk] Walk Packet for existing Player #" + entity.UniqueId);
                entity.MoveTo(location);
            }
            else
            {
                entity = Entity.Spawn(uniqueId, location);
                ThreadedConsole.WriteLine("[Net][MsgWalk] Walk Packet for New Player #" + entity.UniqueId);
                SceneManager.CurrentScene.Entities.Add(entity);
            }

        }
    }
}