using AlumniSocketCore.Queues;
using AlumniSocketCore.Server;
using System;
using System.Collections.Concurrent;
using System.Numerics;
using System.Threading;
using AlumniSocketCore.Client;
using Server.Packets;

namespace Server
{
    public class Program
    {
        public static void Main(string[] args)
        {
            ReceiveQueue.Start(PacketHandler.Handle);
            ServerSocket.Start(13337);

            var t = new Thread(() =>
            {
                while (true)
                {
                    foreach (var kvp in Collections.Players)
                    {
                        var player = kvp.Value;

                        if (DateTime.Now >= player.LastPing.AddSeconds(1))
                        {
                            player.Socket.Send(MsgPing.Create(player.UniqueId));
                            player.LastPing = DateTime.Now;
                        }
                    }

                    Thread.Sleep(1);
                }
            });
            t.Start();
            while (true)
            {
                Console.ReadLine();
            }
        }
    }

    public class Player
    {
        public string Name;
        public uint UniqueId;
        public Vector2 Location;
        public ClientSocket Socket;
        public string Username;
        public string Password;

        public Player(ClientSocket socket)
        {
            Socket = socket;
            Socket.OnConnected += OnConnected;
        }

        public DateTime LastPing { get; set; }

        private void OnConnected()
        {
            Collections.Players.TryRemove(UniqueId, out _);
        }
    }
    public static class Collections
    {
        public static ConcurrentDictionary<uint, Player> Players = new ConcurrentDictionary<uint, Player>();
    }
}