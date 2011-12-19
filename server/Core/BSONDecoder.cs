using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Netronics
{
    class BSONDecoder : PacketDecoder
    {
        public dynamic decode(PacketBuffer buffer)
        {
            if (buffer.legibleBytes() < 5)
                return null;

            buffer.beginBufferIndex();

            UInt32 len = buffer.readUInt32();
            if (len > buffer.legibleBytes())
            {
                buffer.resetBufferIndex();
                return null;
            }

            byte[] data = new byte[len];
            buffer.readBytes(data);

            buffer.endBufferIndex();

            //현재는 byte array를 리턴하지만, 차후 bson 데이터를 리턴하도록 변경.
            return data;
        }
    }
}
