using System.Net.Sockets;
using Netronics.Channel;
using Netronics.Channel.Channel;

namespace Netronics.Template.Service
{
    internal class ServiceChannelFactory : IChannelFactory
    {
        private readonly ServiceManager _manager;
        private readonly Protocol.Protocol _protocol;

        public ServiceChannelFactory(ServiceManager manager)
        {
            _manager = manager;
            _protocol = new Protocol.Protocol(_manager);
        }

        #region IChannelFactory Members

        public IChannel CreateChannel(Netronics netronics, Socket socket)
        {
            SocketChannel channel = SocketChannel.CreateChannel(socket);
            channel.SetParallel(true);
            channel.SetProtocol(_protocol);
            channel.SetHandler(_manager);
            return channel;
        }

        #endregion

    }
}