using Netronics.Channel.Channel;

namespace Netronics.Protocol.PacketEncoder.Http
{
    public class WebSocketEncoder : IPacketEncoder
    {
        public PacketBuffer Encode(IChannel channel, dynamic data)
        {
            var buffer = new PacketBuffer();
            buffer.WriteByte(1);
            buffer.WriteBytes(System.Text.Encoding.UTF8.GetBytes(data));
            buffer.WriteByte(255);
            return buffer;
        }
    }
}
