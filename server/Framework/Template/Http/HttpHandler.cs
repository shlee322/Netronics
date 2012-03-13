using System;
using Netronics.Channel;
using Netronics.Channel.Channel;
using Netronics.Protocol.PacketEncoder.Http;

namespace Netronics.Template.Http
{
    public class HttpHandler : IChannelHandler
    {
        public void AddStatic(string url, string path)
        {
        }

        public void AddDynamic(string url, Func<Request, Response> func)
        {
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
