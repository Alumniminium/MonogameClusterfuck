using AlumniSocketCore.Queues;
using AlumniSocketCore.Server;
using System;
using System.Collections.Concurrent;
using System.Numerics;
using AlumniSocketCore.Client;

namespace Server
{
    public class Program
    {
        public static void Main(string[] args)
        {
            ReceiveQueue.Start(PacketHandler.Handle);
            ServerSocket.Start(1337);

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
        }
    }
    public static class Collections
    {
        public static ConcurrentDictionary<uint, Player> Players = new ConcurrentDictionary<uint, Player>();
    }
}