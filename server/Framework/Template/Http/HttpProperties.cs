using System;
using System.Net;
using Netronics.Channel;
using Netronics.Protocol;
using Netronics.Protocol.PacketEncoder;
using Netronics.Protocol.PacketEncoder.Http;
using Netronics.Protocol.PacketEncryptor;

namespace Netronics.Template.Http
{
    public class HttpProperties : Properties, IProtocol
    {
        private static readonly HttpEncoder Encoder = new HttpEncoder();
        private static readonly HttpDecoder Decoder = new HttpDecoder();

        public HttpProperties(Func<IChannelHandler> handler, int port = 80)
        {
            IpEndPoint = new IPEndPoint(IPAddress.Any, port);
            ((ChannelPipe) ChannelPipe).SetProtocol(()=>this).SetHandler(handler);
        }

        public IPacketEncryptor GetEncryptor()
        {
            return null;
        }

        public IPacketDecryptor GetDecryptor()
        {
            return null;
        }

        public IPacketEncoder GetEncoder()
        {
            return Encoder;
        }

        public IPacketDecoder GetDecoder()
        {
            return Decoder;
        }
    }
}