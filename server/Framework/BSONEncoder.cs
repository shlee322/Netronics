using System;
using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Bson;

namespace Netronics
{
    /// <summary>
    /// BSON을 사용하는 Packet Encoder
    /// 
    ///  BSON 패킷 구조
    /// ┌─────────┬───────────┐
    /// │len = uint(4byte)│BSON DATA([len] byte)│
    /// └─────────┴───────────┘
    ///
    /// </summary>
    /// 
    public class BSONEncoder : PacketEncoder
    {
        protected JsonSerializer serializer = new JsonSerializer();

        #region PacketEncoder Members

        /// <summary>
        /// BSON Encode 함수
        /// 
        /// 입력받는 bson 데이터를 BSON 패킷 구조로 변환
        /// </summary>
        /// <param name="data">BSON Data</param>
        /// <returns>BSON 패킷 구조를 따르는 PacketBuffer</returns>
        public PacketBuffer encode(dynamic data)
        {
            var buffer = new PacketBuffer();

            var stream = new MemoryStream();
            serializer.Serialize(new BsonWriter(stream), data);
            stream.Position = 0;

            buffer.write((UInt32) stream.Length);
            buffer.write(stream);
            stream.Dispose();

            return buffer;
        }

        #endregion
    }
}