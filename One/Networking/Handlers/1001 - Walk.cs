using Microsoft.Xna.Framework;
using One.Entities;
using One.Networking.Packets;
using One.SceneManagement;
using One.Systems;

namespace One.Networking.Handlers
{
    public static class Walk
    {
        public static void Handle(Player player, MsgWalk packet)
        {
            var uniqueId = packet.UniqueId;
            var location = new Vector2(packet.X,packet.Y);
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