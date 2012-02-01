using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using Netronics.Channel;
using Netronics.PacketEncoder.Http;

namespace Netronics.Template.HTTP
{
    class HttpChannelFactory : IChannelFactory
    {
        private readonly HttpProperties _properties;
        public HttpChannelFactory(HttpProperties properties)
        {
            _properties = properties;
        }

        public Channel.Channel CreateChannel(Netronics netronics, Socket socket)
        {
            return Channel.Channel.CreateChannel(socket, new HttpEncoder(), new HttpDecoder(),
                                                 _properties.CreateHandler());
        }
    }
}
