using One.Entities;
using One.Networking.Packets;
using One.Systems;

namespace One.Networking.Handlers
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