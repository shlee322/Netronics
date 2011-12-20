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
            if (buffer.legibleBytes() < 5) //버퍼길이가 5미만이면 리턴
                return null;

            //버퍼 읽기 시작을 알림
            buffer.beginBufferIndex();

            UInt32 len = buffer.readUInt32();
            if (len > buffer.legibleBytes())
            {
                //버퍼의 길이가 실제 패킷 길이보다 모자름으로, 리셋후 리턴
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
