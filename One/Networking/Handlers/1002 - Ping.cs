using MonoGameClusterFuck.Entities;
using MonoGameClusterFuck.Networking.Packets;
using MonoGameClusterFuck.Systems;

namespace MonoGameClusterFuck.Networking.Handlers
{
    public class Ping
    {
        public static void Handle(Player player, byte[] buffer)
        {
            var msgPing = (MsgPing)buffer;

            if (msgPing.Ping == 0)
                player.Socket.Send(msgPing);
            else
            {
                ThreadedConsole.WriteLine("[Net][MsgPing] Ping: " + msgPing.Ping);
                FpsCounter.Ping = msgPing.Ping;
            }
        }
    }
}