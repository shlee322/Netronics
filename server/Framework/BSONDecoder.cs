using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Bson;

namespace Netronics
{
    /// <summary>
    /// BSON을 사용하는 Packet Decoder
    /// 
    ///  BSON 패킷 구조
    /// ┌─────────┬────────────┐
    /// │len = uint(4byte)│ BSON DATA([len] byte)│
    /// └─────────┴────────────┘
    ///
    /// </summary>
    public class BSONDecoder : PacketDecoder
    {
        protected JsonSerializer serializer = new JsonSerializer();

        /// <summary>
        /// BSON 패킷 구조를 따르는 PacketBuffer을 BSON Data로 변환 시키는 메서드
        /// </summary>
        /// <param name="buffer">BSON 패킷 구조를 따르는 Packet Buffer</param>
        /// <returns>BSON Data</returns>
        public dynamic decode(PacketBuffer buffer)
        {
            //버퍼 읽기 시작을 알림
            buffer.beginBufferIndex();

            if (buffer.legibleBytes() < 5) //버퍼길이가 5미만이면 리턴
                return null;

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
