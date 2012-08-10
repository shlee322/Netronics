using System.Net;
using Netronics;
using Netronics.Channel;
using Netronics.Channel.Channel;
using Service.Service.Manager.Handler;

namespace Service.Service.Manager.ServiceChannel
{
    class Client
    {
        private readonly RemoteService _service;
        private readonly Netronics.Client _client;

        public Client(RemoteService service, IPAddress address, int port)
        {
            _service = service;
            _client = new Netronics.Client(new Properties.Properties(new IPEndPoint(address, port), () => new Handler.Service(_service.GetServices().GetManagerProcessor())));
            _client.Start();
        }
    }
}
