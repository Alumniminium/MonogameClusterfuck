using AlumniSocketCore.Client;
using AlumniSocketCore.Queues;
using AlumniSocketCore.Server;
using System;

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

    public class PacketHandler
    {
        public static void Handle(ClientSocket socket, byte[] buffer)
        {
            var packetId = BitConverter.ToUInt16(buffer, 4);
            switch (packetId)
            {
                case 1000:
                    {
                        var msgLogin = (MsgLogin)buffer;
                        var uniqueId = msgLogin.UniqueId;
                        var user = msgLogin.GetUsername();
                        var pass = msgLogin.GetPassword();

                        Console.WriteLine($"Login request for {user} using password {pass}");


                        if (uniqueId != 0)
                            Console.WriteLine("Authentication successful. Your customer Id is: " + uniqueId);
                        else
                            Console.WriteLine("Authentication failed.");
                        break;
                    }
            }
        }
    }
}