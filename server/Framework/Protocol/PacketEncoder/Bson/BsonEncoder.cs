using System;
using System.IO;
using Netronics.Channel;
using Netronics.Channel.Channel;
using Newtonsoft.Json;
using Newtonsoft.Json.Bson;

namespace Netronics.Protocol.PacketEncoder.Bson
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
    public class BsonEncoder : IPacketEncoder
    {
        public static BsonEncoder Encoder = new BsonEncoder();

        protected JsonSerializer Serializer = new JsonSerializer();

        #region IPacketEncoder Members

        /// <summary>
        /// BSON Encode 함수
        /// 
        /// 입력받는 bson 데이터를 BSON 패킷 구조로 변환
        /// </summary>
        /// <param name="data">BSON Data</param>
        /// <returns>BSON 패킷 구조를 따르는 PacketBuffer</returns>
        public PacketBuffer Encode(IChannel channel, dynamic data)
        {
            var buffer = new PacketBuffer();

            var stream = new MemoryStream();
            Serializer.Serialize(new BsonWriter(stream), data);
            stream.Position = 0;

            buffer.Write((UInt32) stream.Length);
            buffer.Write(stream);
            stream.Dispose();

            return buffer;
        }

        #endregion
    }
}