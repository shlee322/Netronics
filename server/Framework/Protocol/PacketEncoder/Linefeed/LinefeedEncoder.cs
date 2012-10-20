using Netronics.Channel.Channel;

namespace Netronics.Protocol.PacketEncoder.Linefeed
{
    public class LinefeedEncoder : IPacketEncoder, IPacketDecoder
    {
        public static LinefeedEncoder Encoder = new LinefeedEncoder(System.Text.Encoding.UTF8);
        private readonly System.Text.Encoding _encoding;
        
        public LinefeedEncoder(System.Text.Encoding encoding)
        {
            _encoding = encoding;
        }

        public PacketBuffer Encode(IChannel channel, object data)
        {
            var str = data as string;
            if (str == null)
                return null;

            var buffer = new PacketBuffer();
            buffer.Write(_encoding.GetBytes(str));
            buffer.WriteBytes(new byte[]{10,13});
            return buffer;
        }

        public object Decode(IChannel channel, PacketBuffer buffer)
        {
            var index = buffer.FindBytes(new byte[] {10, 13});
            return index == -1 ? null : _encoding.GetString(buffer.ReadBytes((int)index));
        }
    }
}
