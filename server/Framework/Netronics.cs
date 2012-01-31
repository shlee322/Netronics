using System;
using System.Net.Sockets;

namespace Netronics
{
    public class Netronics
    {
        private readonly Properties _properties;
        private Socket _socket;
        
        public Netronics(Properties properties)
        {
            _properties = properties;
        }

        public void Start()
        {
            InitSocket();
            StartSocket();
            _properties.OnStartEvent(this, new EventArgs());
        }

        public void Stop()
        {
            _properties.OnStopEvent(this, new EventArgs());
        }

        private void InitSocket()
        {
            _socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            _socket.Bind(_properties.IpEndPoint);
        }

        private void StartSocket()
        {
            _socket.Listen(50);
            _socket.BeginAccept(AcceptCallback, null);
        }

        private void AcceptCallback(IAsyncResult ar)
        {
            _properties.ChannelFactory.NewChannel(this, _socket.EndAccept(ar));
            _socket.BeginAccept(AcceptCallback, null);
        }
    }
}