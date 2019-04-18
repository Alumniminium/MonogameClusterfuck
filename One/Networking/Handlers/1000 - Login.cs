using System;
using Microsoft.Xna.Framework;
using MonoGameClusterFuck;
using MonoGameClusterFuck.Entities;
using MonoGameClusterFuck.Networking.Packets;

namespace One.Networking.Handlers
{
    public static class Login
    {
        internal static void Handle(Player player, MsgLogin packet)
        {
            player.UniqueId = packet.UniqueId;

            if(player.UniqueId == 0)
            {
                    MonoGameClusterFuck.Engine.SpriteBatch.DrawString(Fonts.Generic,"U GOT DISCONNECTED M8",Vector2.Zero, Color.Red);
            }
            else
            {
                    MonoGameClusterFuck.Engine.SpriteBatch.DrawString(Fonts.Generic,"U GOT CONNECTED M8",Vector2.Zero, Color.Red);
            }
        }
    }
}