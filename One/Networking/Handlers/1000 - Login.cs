using MonoGameClusterFuck.Entities;
using MonoGameClusterFuck.Networking.Packets;
using MonoGameClusterFuck.Systems;

namespace MonoGameClusterFuck.Networking.Handlers
{
    public static class Login
    {
        internal static void Handle(Player player, MsgLogin packet)
        {
            player.UniqueId = packet.UniqueId;
            var (user,pass) = packet.GetUserPass();
            ThreadedConsole.WriteLine("[Net][MsgLogin] Login Packet for Player " + user + " using password: "+pass);
            if (player.UniqueId == 0)
            {
                ThreadedConsole.WriteLine("[Net][MsgLogin] " + user + " failed to authenticate! (not implemented)");
            }
            else
            {
                ThreadedConsole.WriteLine("[Net][MsgLogin] " + user + " authenticated! (not implemented)");
            }
        }
    }
}