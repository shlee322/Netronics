using Netronics;
using Netronics.Channel.Channel;
using Netronics.Protocol.PacketEncoder;

namespace BroadcastServer
{
    class PacketEncoder : IPacketEncoder, IPacketDecoder
    {
        public PacketBuffer Encode(IChannel channel, dynamic data)
        {
            if (data.GetType() != typeof(string))
                return null;

            byte[] bytes = data;
            PacketBuffer buffer = new PacketBuffer();
            buffer.Write(bytes, 0, bytes.Length);
            return buffer;
        }

        public dynamic Decode(IChannel channel, PacketBuffer buffer)
        {
            buffer.BeginBufferIndex();
            if (buffer.AvailableBytes() < 1)
            {
                buffer.ResetBufferIndex();
                return null;
            }

            var data = new byte[buffer.AvailableBytes()];
            buffer.ReadBytes(data);
            buffer.EndBufferIndex();
            return data;
        }
    }
}
