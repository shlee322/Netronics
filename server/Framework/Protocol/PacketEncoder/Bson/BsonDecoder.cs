﻿using System.IO;
using Netronics.Channel;
using Netronics.Channel.Channel;
using Newtonsoft.Json;
using Newtonsoft.Json.Bson;

namespace Netronics.Protocol.PacketEncoder.Bson
{
    /// <summary>
    /// BSON을 사용하는 Packet Decoder
    /// 
    ///  BSON 패킷 구조
    /// ┌──────┬─────────┬───────────┐
    /// │ver = 1byte│len = uint(4byte)│BSON DATA([len] byte)│
    /// └──────┴─────────┴───────────┘
    ///
    /// </summary>
    public class BsonDecoder : IPacketDecoder
    {
        public static BsonDecoder Decoder = new BsonDecoder();

        protected JsonSerializer Serializer = new JsonSerializer();

        #region IPacketDecoder Members

        /// <summary>
        /// BSON 패킷 구조를 따르는 PacketBuffer을 BSON Data로 변환 시키는 메서드
        /// </summary>
        /// <param name="buffer">BSON 패킷 구조를 따르는 Packet Buffer</param>
        /// <returns>BSON Data</returns>
        public dynamic Decode(IChannel channel, PacketBuffer buffer)
        {
            //버퍼 읽기 시작을 알림
            buffer.BeginBufferIndex();

            if (buffer.AvailableBytes() < 6) //버퍼길이가 5미만이면 리턴
                return null;
            buffer.ReadByte();
            uint len = buffer.ReadUInt32();
            if (len > buffer.AvailableBytes())
            {
                //버퍼의 길이가 실제 패킷 길이보다 모자름으로, 리셋후 리턴
                buffer.ResetBufferIndex();
                return null;
            }

            var data = new byte[len];
            buffer.ReadBytes(data);

            buffer.EndBufferIndex();

            var stream = new MemoryStream(data);
            dynamic res = Serializer.Deserialize(new BsonReader(stream));
            stream.Dispose();

            return res;
        }

        #endregion
    }
}