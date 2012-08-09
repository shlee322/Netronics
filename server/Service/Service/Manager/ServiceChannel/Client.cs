using System.Net;
using Netronics;
using Netronics.Channel;
using Netronics.Channel.Channel;
using Service.Service.Manager.Handler;

namespace Service.Service.Manager.ServiceChannel
{
    class Client : IChannelHandler
    {
        private readonly RemoteService _service;
        private readonly Netronics.Client _client;
        private IChannel _channel;

        public Client(RemoteService service, IPAddress address, int port)
        {
            _service = service;
            _client = new Netronics.Client(new Properties.Properties(new IPEndPoint(address, port), () => this));
            _client.Start();
        }

        public void Connected(IChannel channel)
        {
            _channel = channel;
            
            //이제 내 정보를 알린다.
        }

        public void Disconnected(IChannel channel)
        {
            _channel = null;
        }

        public void MessageReceive(IChannel channel, dynamic message)
        {
            ServiceMessageReceiver.Receive(_service.GetServices().GetManagerProcessor(), channel, message);
        }
    }
}
