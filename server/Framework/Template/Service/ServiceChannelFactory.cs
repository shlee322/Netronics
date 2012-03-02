﻿using System.Net.Sockets;
using Netronics.Channel;
using Netronics.Channel.Channel;

namespace Netronics.Template.Service
{
    internal class ServiceChannelFactory : IChannelFactory
    {
        private readonly ServiceManager _manager;

        public ServiceChannelFactory(ServiceManager manager)
        {
            _manager = manager;
        }

        #region IChannelFactory Members

        public IChannel CreateChannel(Netronics netronics, Socket socket)
        {
            SocketChannel channel = SocketChannel.CreateChannel(socket);
            channel.SetParallel(true);
            channel.SetProtocol(Protocol.Protocol.GetInstance());
            channel.SetHandler(_manager);
            return channel;
        }

        #endregion

    }
}