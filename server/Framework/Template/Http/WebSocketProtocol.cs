using Netronics.Channel.Channel;
using Netronics.Protocol;
using Netronics.Protocol.PacketEncoder;
using Netronics.Protocol.PacketEncoder.Http;
using Netronics.Protocol.PacketEncryptor;

namespace Netronics.Template.Http
{
    class WebSocketProtocol : IProtocol
    {
        public static readonly WebSocketProtocol Protocol = new WebSocketProtocol();
        private static readonly IPacketEncoder Encoder = new WebSocketEncoder();
        private static readonly IPacketDecoder Decoder = new WebSocketDecoder();

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
