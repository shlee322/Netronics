using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Bson;

namespace Netronics
{
    public class BSONDecoder : PacketDecoder
    {
        protected JsonSerializer serializer = new JsonSerializer();
        public dynamic decode(PacketBuffer buffer)
        {
            if (buffer.legibleBytes() < 9) //버퍼길이가 5미만이면 리턴
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



            return serializer.Deserialize(new BsonReader(new MemoryStream(data)));
        }
    }
}
