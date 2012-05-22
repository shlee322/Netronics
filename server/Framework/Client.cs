using System.Net.Sockets;
using Netronics.Channel.Channel;

namespace Netronics
{
    public class Client : Netronics
    {
        private IChannel _channel;
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
            _channel = AddChannel(Properties.GetChannelFactory().CreateChannel(this, Socket));
            _channel.Connect();
        }

        public IChannel GetChannel()
        {
            return _channel;
        }
    }
}
