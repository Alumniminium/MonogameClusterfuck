using MonoGameClusterFuck;
using MonoGameClusterFuck.Entities;
using MonoGameClusterFuck.Networking.Packets;
using MonoGameClusterFuck.Scenes;

namespace One.Networking.Handlers
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
                entity.MoveTo(location);
            }
            else
            {
                entity = Entity.Spawn(uniqueId, location);
                SceneManager.CurrentScene.Entities.Add(entity);
            }

        }
    }
}