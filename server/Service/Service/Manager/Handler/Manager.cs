﻿using Netronics.Channel;
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

        public void Connected(IChannel channel)
        {
            channel.SendMessage(_processor.GetJoinServicePacket());
        }

        public void Disconnected(IChannel channel)
        {
        }

        public void MessageReceive(IChannel channel, dynamic message)
        {
            if(message.type == "notify_join_service")
                _processor.NotifyJoinService((string)message.service, (int)message.id, (byte[])message.address, (int)message.port);
            
        }
    }
}