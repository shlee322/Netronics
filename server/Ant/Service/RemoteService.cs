using System.Collections.Generic;
using System.Net;
using Netronics.Channel.Channel;
using Service.Service.Manager.ServiceChannel;

namespace Service.Service
{
    class RemoteService : Service
    {
        private List<Client> _clients = new List<Client>();
        private List<IChannel> _channels = new List<IChannel>();
        private Services _services;
        private int _id;

        public RemoteService(Services services, int id)
        {
            _services = services;
            _id = id;
        }

        public Services GetServices()
        {
            return _services;
        }

        public void AddNetwork(byte[] address, int port)
        {
            _clients.Add(new Client(this, new IPAddress(address), port));
        }

        public void AddChannel(IChannel channel)
        {
            _channels.Add(channel);
        }
    }
}
