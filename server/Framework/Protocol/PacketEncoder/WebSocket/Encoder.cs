using System;
using System.IO;
using Netronics.Channel.Channel;

namespace Netronics.Protocol.PacketEncoder.WebSocket
{
    public class Encoder : IPacketEncoder
    {
        private void InputBuffer(PacketBuffer buffer, byte type, byte[] data)
        {
            var buf = new byte[127];
            var stream = new MemoryStream(data);

            while (true)
            {
                int len = stream.Read(buf, 0, 127);
                if (len == 0)
                    break;

                buffer.WriteByte((byte)((len != 127 ? 0x80 : 0x0) | type));
                buffer.WriteByte((byte)(0x7F & len));

                buffer.Write(buf, 0, len);

                if (len != 127)
                    break;
            }
        }

        public PacketBuffer Encode(IChannel channel, dynamic data)
        {
            var buffer = new PacketBuffer();
            if (data is string)
                InputBuffer(buffer, 1, System.Text.Encoding.UTF8.GetBytes(data));
            else if (data is byte[])
                InputBuffer(buffer, 2, data);
            else if (data is Ping)
                return null;
            else if (data is Pong)
                return null;
            else
                return null;
            return buffer;
        }
    }
}
