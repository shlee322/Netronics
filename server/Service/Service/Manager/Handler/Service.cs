using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Netronics.Channel;
using Netronics.Channel.Channel;

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
        }

        public void Disconnected(IChannel channel)
        {
        }

        public void MessageReceive(IChannel channel, dynamic message)
        {
        }
    }
}
