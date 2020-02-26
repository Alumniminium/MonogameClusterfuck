using AlumniSocketCore.Queues;
using AlumniSocketCore.Server;
using System;
using System.Threading;
using Server.Packets;

namespace Server
{
    public class Program
    {
        public static void Main(string[] args)
        {
            ReceiveQueue.Start(PacketHandler.Handle);
            ServerSocket.Start(13338);
            Console.WriteLine($"Server running!");

            var t = new Thread(() =>
            {
                Console.WriteLine($"Heartbeat Thread started.");
                while (true)
                {
                    foreach (var kvp in Collections.Players)
                    {
                        var player = kvp.Value;

                        if (DateTime.Now >= player.LastPing.AddSeconds(1))
                        {
                            Console.WriteLine($"Sending Ping to {player.Name}/{player.Username}.");
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
}