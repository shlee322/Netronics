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

        public void Connected(IReceiveContext context)
        {
        }

        public void Disconnected(IReceiveContext context)
        {
        }

        public void MessageReceive(IReceiveContext context)
        {
            dynamic message = context.GetMessage();
            if(message.type == "join_service")
                _manager.JoinService(context.GetChannel(), (string)message.name, message.id == null ? -1 : (int)message.id, (JArray)message.address, (int)message.port);
            
        }
    }
}
