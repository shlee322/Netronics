using System;
using System.Net;
using System.Net.Sockets;
using Netronics.Channel;
using Netronics.Channel.Channel;

namespace Netronics
{
    public class Netronics
    {
        private readonly IProperties _properties;
        private Socket _socket;

        public Netronics(IProperties properties)
        {
            _properties = properties;
        }

        public Netronics Start()
        {
            if (_properties == null)
                return this;

            InitSocket();
            StartSocket();
            _properties.OnStartEvent(this, new EventArgs());

            return this;
        }

        public Netronics Stop()
        {
            if (_properties != null)
                _properties.OnStopEvent(this, new EventArgs());
            return this;
        }

        private void InitSocket()
        {
            _socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            _socket.Bind(_properties.GetIPEndPoint());
        }

        private void StartSocket()
        {
            _socket.Listen(50);
            _socket.BeginAccept(AcceptCallback, null);
        }

        public IPEndPoint GetEndIPPoint()
        {
            return (IPEndPoint) _socket.LocalEndPoint;
        }

        private void AcceptCallback(IAsyncResult ar)
        {
            AddChannel(_properties.GetChannelFactory().CreateChannel(this, _socket.EndAccept(ar))).Connect();
            _socket.BeginAccept(AcceptCallback, null);
        }

        public IChannel AddChannel(IChannel channel)
        {
            return channel;
        }

        public IChannel AddSocket(Socket socket)
        {
            return AddChannel(_properties.GetChannelFactory().CreateChannel(this, socket));
        }
    }
}