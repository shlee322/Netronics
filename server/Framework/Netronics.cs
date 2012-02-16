﻿using System;
using System.Net.Sockets;
using Netronics.Channel;

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

            Scheduler.Add(null); //스케줄러 초기화를 위해 호출
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

        private void AcceptCallback(IAsyncResult ar)
        {
            AddChannel(_properties.GetChannelFactory().CreateChannel(this, _socket.EndAccept(ar))).Connect();
            _socket.BeginAccept(AcceptCallback, null);
        }

        public IChannel AddChannel(IChannel channel)
        {
            return channel;
        }
    }
}