using MonoGameClusterFuck.Entities;
using MonoGameClusterFuck.Networking.Packets;

namespace MonoGameClusterFuck.Networking.Handlers
{
    public static class Login
    {
        internal static void Handle(Player player, MsgLogin packet)
        {
            player.UniqueId = packet.UniqueId;

            if(player.UniqueId == 0)
            {

            }
            else
            {

            }
        }
    }
}