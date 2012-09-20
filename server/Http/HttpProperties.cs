using System;
using System.Net;
using Netronics.Channel;
using Netronics.Protocol.PacketEncoder.Http;

namespace Netronics.Template.Http
{
    public class HttpProperties : Properties
    {
        private static readonly HttpEncoder Encoder = new HttpEncoder();
        private static readonly HttpDecoder Decoder = new HttpDecoder();

        public HttpProperties(Func<IChannelHandler> handler, int port = 80)
        {
            IpEndPoint = new IPEndPoint(IPAddress.Any, port);
            ((ChannelPipe) ChannelPipe).SetCreateChannelAction((channel)=>
                {
                    channel.SetConfig("encoder", Encoder);
                    channel.SetConfig("decoder", Decoder);
                    channel.SetConfig("handler", handler());
                });
        }
    }
}