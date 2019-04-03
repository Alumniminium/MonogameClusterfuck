using AlumniSocketCore.Client;
using AlumniSocketCore.Queues;

namespace MonoGameClusterFuck.Networking
{
    public class Client
    {
        public ClientSocket Socket;
        public string Ip = "192.168.0.3";
        public ushort Port = 65534;
        public bool IsConnected;


        public void ConnectAsync(string ip, ushort port)
        {
            ReceiveQueue.Start(OnPacket);
            Socket = new ClientSocket(this);
            Socket.OnDisconnect += Disconnected;
            Socket.OnConnected += Connected;
            Socket.Connect(ip, port);
        }

        private void Connected() => IsConnected = true;

        private void Disconnected() => ConnectAsync(Ip, Port);

        private void OnPacket(ClientSocket client, byte[] buffer) => PacketHandler.Handle((Client)client.StateObject, buffer);

        public void Send(byte[] packet) => Socket.Send(packet);
    }
}
