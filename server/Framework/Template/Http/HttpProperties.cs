using System;
using System.Net;
using Netronics.Channel;
using Netronics.PacketEncoder.Http;

namespace Netronics.Template.HTTP
{
    public class HttpProperties : Properties
    {
        public HttpProperties()
        {
            IpEndPoint = new IPEndPoint(IPAddress.Any, 80);
            ((BasicChannelFactory)ChannelFactory).SetPacketEncoder(() => new HttpEncoder()).SetPacketDecoder(
                () => new HttpDecoder());
        }

        public HttpProperties SetHandler(Func<IChannelHandler> handler)
        {
            ((BasicChannelFactory) ChannelFactory).SetHandler(handler);
            return this;
        }
    }
}
