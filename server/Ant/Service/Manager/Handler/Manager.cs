using Netronics.Channel;
using Netronics.Channel.Channel;

namespace Service.Service.Manager.Handler
{
    class Manager : IChannelHandler
    {
        private ManagerProcessor _processor;

        public Manager(ManagerProcessor processor)
        {
            _processor = processor;
        }

        public void Connected(IReceiveContext context)
        {
            context.GetChannel().SendMessage(_processor.GetJoinServicePacket());
        }

        public void Disconnected(IReceiveContext context)
        {
        }

        public void MessageReceive(IReceiveContext context)
        {
            dynamic message = context.GetMessage();
            if (message.type == "notify_join_service")
                _processor.NotifyJoinService((string)message.service, (int)message.id, (byte[])message.address, (int)message.port);
            else if (message.type == "change_service_id")
                _processor.ChangeServiceId((int)message.id);
            else if (message.type == "max_entity_id")
                _processor.ChangeMaxEntityId((long)message.value);
        }
    }
}
