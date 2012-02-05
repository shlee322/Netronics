using Netronics;
using Netronics.PacketEncoder;

namespace EchoServer
{
    class PacketDecoder : IPacketDecoder
    {
        public dynamic Decode(PacketBuffer buffer)
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
