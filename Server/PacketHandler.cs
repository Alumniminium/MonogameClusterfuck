using System;
using AlumniSocketCore.Client;
using Server.Packets;

namespace Server
{
    public static class PacketHandler
    {
        public static void Handle(ClientSocket socket, byte[] buffer)
        {
            var packetId = BitConverter.ToUInt16(buffer, 4);
            switch (packetId)
            {
                case 1000:
                {
                    var msgLogin = (MsgLogin)buffer;
                    var user = msgLogin.GetUsername();
                    var pass = msgLogin.GetPassword();

                    Console.WriteLine($"Login request for {user} using password {pass}");

                    msgLogin.UniqueId = (uint)Core.Random.Next(0, 10000);
                    var player = new Player(socket)
                    {
                        UniqueId = msgLogin.UniqueId,
                        Username = user,
                        Password = pass
                    };
                    socket.StateObject = player;
                    Collections.Players.TryAdd(msgLogin.UniqueId, player);

                    if (msgLogin.UniqueId != 0)
                        Console.WriteLine("Authentication successful. Your customer Id is: " + msgLogin.UniqueId);
                    else
                        Console.WriteLine("Authentication failed.");

                    player.Socket.Send(msgLogin);

                    break;
                }
                case 1001:
                {
                    var msgWalk = (MsgWalk)buffer;
                    var player = (Player)socket.StateObject;
                    player.Location = msgWalk.Location;
                    msgWalk.TickCount = Environment.TickCount;
                    Console.WriteLine($"Player: {player.Username} ({msgWalk.UniqueId}) moved to: {player.Location.X},{player.Location.Y}");

                    foreach (var kvp in Collections.Players)
                    {
                        kvp.Value.Socket.Send(msgWalk);
                    }

                    break;
                }
                case 1002:
                {
                    var msgPing = (MsgPing)buffer;
                    var delta = DateTime.UtcNow.Ticks - msgPing.TickCount;
                    var ms = delta / 10000;
                    msgPing.Ping = (short)ms;
                    socket.Send(msgPing);
                    break;
                }
            }
        }
    }

    public static class Core
    {
        public static Random Random = new Random();
    }
}