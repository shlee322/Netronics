using System;
using System.IO;
using MsgPack;
using Netronics.Channel.Channel;

namespace Netronics.Protocol.PacketEncoder.MsgPack
{
    public class MsgPackEncoder : IPacketEncoder
    {
        public static MsgPackEncoder Encoder = new MsgPackEncoder();

        public PacketBuffer Encode(IChannel channel, object data)
        {
            if (!(data is MessagePackObject))
                return null;
            var buffer = new PacketBuffer();
            buffer.WriteByte(1);
            var stream = new MemoryStream();
            Packer.Create(stream).Pack((MessagePackObject)data);
            stream.Position = 0;

            buffer.Write((UInt32)stream.Length);
            buffer.Write(stream);
            stream.Dispose();

            return buffer;
        }
    }
}
