using AlumniSocketCore.Client;
using AlumniSocketCore.Queues;
using MonoGameClusterFuck.Entities;

namespace MonoGameClusterFuck.Networking
{
    public class NetworkClient
    {
        public ClientSocket Socket;
        public Player Player;
        public string Ip = "192.168.0.3";
        public ushort Port = 65534;
        public bool IsConnected;
        public int LastUpdateTick;

        public NetworkClient(Player player)
        {
            Player = player;
        }

        public void ConnectAsync(string ip, ushort port)
        {
            ReceiveQueue.Start(OnPacket);
            Socket = new ClientSocket(this);
            Socket.OnDisconnect += Disconnected;
            Socket.OnConnected += Connected;
            Socket.ConnectAsync(ip, port);
        }

        private void Connected() => IsConnected = true;

        private void Disconnected() => ConnectAsync(Ip, Port);

        private void OnPacket(ClientSocket client, byte[] buffer) => PacketHandler.Handle((NetworkClient)client.StateObject, buffer);

        public void Send(byte[] packet) => Socket.Send(packet);
    }
}
