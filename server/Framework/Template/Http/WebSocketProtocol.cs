using Netronics.Protocol;
using Netronics.Protocol.PacketEncoder;
using Netronics.Protocol.PacketEncoder.WebSocket;
namespace Netronics.Template.Http
{
    class WebSocketProtocol : IProtocol
    {
        public static readonly WebSocketProtocol Protocol = new WebSocketProtocol();
        private static readonly IPacketEncoder Encoder = new Encoder();
        private static readonly IPacketDecoder Decoder = new Decoder();


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
