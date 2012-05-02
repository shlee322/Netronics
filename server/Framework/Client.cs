using System.Net.Sockets;

namespace Netronics
{
    public class Client : Netronics
    {
        public Client(IProperties properties) : base(properties)
        {
        }

        protected override void InitSocket()
        {
            Socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        }

        protected override void StartSocket()
        {
            Socket.Connect(Properties.GetIPEndPoint());
            AddChannel(Properties.GetChannelFactory().CreateChannel(this, Socket)).Connect();
        }
    }
}
