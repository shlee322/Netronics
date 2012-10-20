using System.Net.Sockets;
using Netronics.Channel.Channel;

namespace Netronics
{
    /// <summary>
    /// Netronics를 클라이언트용으로 사용하기 위한 클래스
    /// 단 한개만의 Channel을 가지고 있다.
    /// </summary>
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
            _channel = AddChannel(Properties.GetChannelPipe().CreateChannel(this, Socket));
            if (_channel == null)
                return;
            _channel.Connect();
        }

        /// <summary>
        /// Channel을 반환하는 메소드
        /// </summary>
        /// <returns>Channel</returns>
        public IChannel GetChannel()
        {
            return _channel;
        }
    }
}