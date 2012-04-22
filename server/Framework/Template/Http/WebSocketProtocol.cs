using Netronics.Protocol;
using Netronics.Protocol.PacketEncoder;
using Netronics.Protocol.PacketEncryptor;

namespace Netronics.Template.Http
{
    class WebSocketProtocol : IProtocol
    {
        public static readonly WebSocketProtocol Protocol = new WebSocketProtocol(); 

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
            return null;
        }

        public IPacketDecoder GetDecoder()
        {
            return null;
        }
    }
}
