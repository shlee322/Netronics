using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Netronics.Channel;
using Netronics.Channel.Channel;
using Netronics.Protocol.PacketEncoder.Http;

namespace WebServer
{
    class WebSocketHandler : IChannelHandler
    {
        public void Connected(IChannel channel)
        {
        }

        public void Disconnected(IChannel channel)
        {
        }

        public void MessageReceive(IChannel channel, dynamic message)
        {
            channel.SendMessage(message);
        }
    }
}
