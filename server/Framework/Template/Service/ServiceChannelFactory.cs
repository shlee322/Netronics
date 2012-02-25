using System.Net.Sockets;
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
            return SocketChannel.CreateChannel(socket);
        }

        #endregion

    }
}