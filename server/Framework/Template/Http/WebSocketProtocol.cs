using Netronics.Channel.Channel;
using Netronics.Protocol;
using Netronics.Protocol.PacketEncoder;
using Netronics.Protocol.PacketEncryptor;

namespace Netronics.Template.Http
{
    class WebSocketProtocol : IProtocol, IPacketEncoder, IPacketDecoder
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
            return this;
        }

        public IPacketDecoder GetDecoder()
        {
            return this;
        }

        public PacketBuffer Encode(IChannel channel, dynamic data)
        {
            var buffer = new PacketBuffer();
            buffer.WriteByte(0);
            buffer.WriteBytes(System.Text.Encoding.UTF8.GetBytes(data));
            buffer.WriteByte(255);
            return buffer;
        }

        public dynamic Decode(IChannel channel, PacketBuffer buffer)
        {
            return null;
        }
    }
}
