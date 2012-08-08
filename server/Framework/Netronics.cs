using System;
using System.Net;
using System.Net.Sockets;
using Netronics.Channel;
using Netronics.Channel.Channel;
using Netronics.Event;

namespace Netronics
{
    public class Netronics
    {
        protected readonly IProperties Properties;
        protected Socket Socket;

        public Netronics(IProperties properties)
        {
            Properties = properties;
        }

        public virtual Netronics Start()
        {
            if (Properties == null)
                return this;

            InitSocket();
            StartSocket();
            Properties.OnStartEvent(this, new StartEventArgs(Socket));

            return this;
        }

        public virtual Netronics Stop()
        {
            if (Properties != null)
                Properties.OnStopEvent(this, new EventArgs());
            return this;
        }

        protected virtual void InitSocket()
        {
            Socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        }

        protected virtual void StartSocket()
        {
            Socket.Bind(Properties.GetIPEndPoint());
            Socket.Listen(50);
            Socket.BeginAccept(AcceptCallback, null);
        }

        public virtual IPEndPoint GetEndIPPoint()
        {
            return (IPEndPoint) Socket.LocalEndPoint;
        }

        protected virtual void AcceptCallback(IAsyncResult ar)
        {
            AddChannel(Properties.GetChannelPipe().CreateChannel(this, Socket.EndAccept(ar))).Connect();
            Socket.BeginAccept(AcceptCallback, null);
        }

        public virtual IChannel AddChannel(IChannel channel)
        {
            return channel;
        }

        public virtual IChannel AddSocket(Socket socket)
        {
            return AddChannel(Properties.GetChannelPipe().CreateChannel(this, socket));
        }
    }
}