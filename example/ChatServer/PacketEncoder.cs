using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Netronics;
using Netronics.Channel.Channel;
using Netronics.Protocol.PacketEncoder;

namespace ChatServer
{

    class PacketEncoder : IPacketEncoder, IPacketDecoder
    {
        public PacketBuffer Encode(IChannel channel, dynamic data)
        {
            if (data.GetType() != typeof(string))
                return null;

            byte[] bytes = System.Text.Encoding.UTF8.GetBytes(data);
            PacketBuffer buffer = new PacketBuffer();
            buffer.WriteUInt32((uint)bytes.Length);
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

            string s = System.Text.Encoding.UTF8.GetString(data);
            int len = s.IndexOf('\n');
            if (len == -1)
            {
                buffer.ResetBufferIndex();
                return null;
            }
            s = s.Substring(0, len + 1);

            buffer.SetPosition(System.Text.Encoding.UTF8.GetByteCount(s));
            buffer.EndBufferIndex();

            return s;
        }
    }
}
