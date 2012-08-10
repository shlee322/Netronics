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

        public void Connected(IChannel channel)
        {
            dynamic packet = new JObject();
            packet.type = "connect_service_info";
            packet.service = _processor.GetServiceLoader().GetServiceName();
            packet.id = _processor.GetServiceId();
            channel.SendMessage(packet);
        }

        public void Disconnected(IChannel channel)
        {
        }

        public void MessageReceive(IChannel channel, dynamic message)
        {
            if (message.type == "connect_service_info")
                _processor.ConnectServiceInfo(channel, (string)message.service, (int)message.id);
        }
    }
}
