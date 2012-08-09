using System.Net;
using Netronics.Channel;
using Netronics.Channel.Channel;
using Newtonsoft.Json.Linq;

namespace Service.Manager
{
    class Handler : IChannelHandler
    {
        private ServiceManager _manager;

        public Handler(ServiceManager manager)
        {
            _manager = manager;
        }

        public void Connected(IChannel channel)
        {
        }

        public void Disconnected(IChannel channel)
        {
        }

        public void MessageReceive(IChannel channel, dynamic message)
        {
            if(message.type == "join_service")
                _manager.JoinService(channel, (string)message.name, message.id == null ? -1 : (int)message.id, (JArray)message.address, (int)message.port);
            
        }
    }
}
