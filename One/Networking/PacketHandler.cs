using System;
using MonoGameClusterFuck.Networking.Packets;
using MonoGameClusterFuck.Systems;
using One.Networking.Handlers;

namespace MonoGameClusterFuck.Networking
{
    public static class PacketHandler
    {
        public static void Handle(NetworkClient socket, byte[] buffer)
        {
            var packetId = BitConverter.ToUInt16(buffer, 4);
            var player = socket.Player;
            switch (packetId)
            {
                case 1000:
                    {
                        Login.Handle(player, buffer);
                        break;
                    }
                case 1001:
                    {
                        Walk.Handle(player, buffer);
                        break;
                    }
                case 1002:
                    {
                        var msgPing = (MsgPing)buffer;

                        if (msgPing.Ping == 0)
                            socket.Send(msgPing);
                        else
                            FpsCounter.Ping = msgPing.Ping;

                        break;
                    }
            }
        }
    }
}
