using Netronics.Channel;
using Netronics.Channel.Channel;
using Newtonsoft.Json.Linq;

namespace Service.Service.Manager.Handler
{
    class Service : IChannelHandler
    {
        private ManagerProcessor _processor;

        public Service(ManagerProcessor processor)
        {
            _processor = processor;
        }

        public void Connected(IReceiveContext context)
        {
            dynamic packet = new JObject();
            packet.type = "connect_service_info";
            packet.service = _processor.GetServiceLoader().GetServiceName();
            packet.id = _processor.GetServiceId();
            context.GetChannel().SendMessage(packet);
        }

        public void Disconnected(IReceiveContext context)
        {
        }

        public void MessageReceive(IReceiveContext context)
        {
            dynamic message = context.GetMessage();
            if (message.type == "connect_service_info")
                _processor.ConnectServiceInfo(context.GetChannel(), (string)message.service, (int)message.id);
        }
    }
}
