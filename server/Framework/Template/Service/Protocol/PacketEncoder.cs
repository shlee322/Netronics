using System;
using System.IO;
using System.Reflection;
using Netronics.Channel.Channel;
using Netronics.Protocol.PacketEncoder;
using Netronics.Template.Service.Service;
using Newtonsoft.Json;
using Newtonsoft.Json.Bson;

namespace Netronics.Template.Service.Protocol
{
    /// <summary>
    /// 기본적인 패킷 구조는 다음과 같다.
    /// 
    /// Service 끼리는 신뢰할 수 있는 네트워크에 소속되있다는 전제조건하에 개발되었기때문에, Service끼리의 인증등에 관한 프로토콜은 존재하지 않는다. (차후 추가)
    /// 
    /// 아이디 알림 : 패킷타입(1byte), ID(4byte)
    /// 요청 : 패킷타입(1byte), 요청서비스ID(4byte), 응답서비스ID(4byte), 트랜젝션ID(8byte), 메시지 클래스 이름 길이(2byte), 메시지 클래스 이름(메시지 클래스 이름 길이 byte), data길이(4byte), data
    /// 응답 : 패킷타입(1byte), 응답서비스ID(4byte), 요청서비스ID(4byte), 트랜젝션ID(8byte), 결과 클래스 이름 길이(2byte), 결과 클래스 이름(결과 클래스 이름 길이 byte), data길이(4byte), data
    /// 
    /// 패킷타입
    /// 
    /// 00000000 : 요청 (결과 수신)
    /// 00000001 : 요청 (결과 수신 안함)
    /// 00000010 : 결과 (성공)
    /// 00000011 : 결과 (실패)
    /// 00000100 : ID 알림
    /// </summary>
    class PacketEncoder : IPacketEncoder, IPacketDecoder
    {
        private JsonSerializer _serializer = new JsonSerializer();
        private ServiceManager _manager;

        public PacketEncoder(ServiceManager manager)
        {
            _manager = manager;
        }

        public PacketBuffer Encode(IChannel channel, dynamic data)
        {
            if (data is Request)
                return RequestEncode(data);
            if (data is IDInfo)
                return IDInfoEncode(data);
            return null;
        }

        public dynamic Decode(IChannel channel, PacketBuffer buffer)
        {
            buffer.BeginBufferIndex();

            if (buffer.AvailableBytes() < 1)
                return null;

            byte type = buffer.ReadByte();

            if(type==0x00 || type == 0x01)
            {
                var request = RequestDecode(buffer);
                if (request == null)
                {
                    buffer.ResetBufferIndex();
                    return null;
                }
                buffer.EndBufferIndex();
                return request;
            }

            if(type==0x04)
            {
                var info = IDInfoDecode(buffer);
                if(info==null)
                {
                    buffer.ResetBufferIndex();
                    return null;
                }
                buffer.EndBufferIndex();
                return info;
            }

            buffer.EndBufferIndex();
            return null;
        }

        private PacketBuffer RequestEncode(Request data)
        {
            var buffer = new PacketBuffer();
            buffer.Write(new byte[] { 0 }, 0, 1);
            buffer.Write(data.Sender, 0, 4);
            buffer.Write(data.Receiver, 0, 4);
            buffer.Write(data.Transaction);
            byte[] msg = System.Text.Encoding.UTF8.GetBytes(data.Message.GetType().FullName);
            buffer.Write((ushort)msg.Length);
            buffer.Write(msg, 0, msg.Length);
            MemoryStream ms = new MemoryStream();
            BsonWriter writer = new BsonWriter(ms);
            _serializer.Serialize(writer, data.Message);
            byte[] msgData = ms.ToArray();
            buffer.Write((uint)msgData.Length);
            buffer.Write(msgData, 0, msgData.Length);
            return buffer;
        }

        private Request RequestDecode(PacketBuffer buffer)
        {
            if (buffer.AvailableBytes() < 24)
                return null;
            var request = new Request
                              {
                                  Sender = buffer.ReadBytes(4),
                                  Receiver = buffer.ReadBytes(4),
                                  Transaction = buffer.ReadUInt64()
                              };

            ushort msgLen = buffer.ReadUInt16();
            if (buffer.AvailableBytes() < msgLen)
                return null;
            Type messageType = _manager.GetMessageType(System.Text.Encoding.UTF8.GetString(buffer.ReadBytes(msgLen)));
            uint dataLen = buffer.ReadUInt32();
            if (buffer.AvailableBytes() < dataLen)
                return null;
            MemoryStream ms = new MemoryStream(buffer.ReadBytes((int) dataLen));
            BsonReader reader = new BsonReader(ms);
            request.Message = _serializer.Deserialize(reader, messageType);
            return request;
        }

        private static PacketBuffer IDInfoEncode(IDInfo info)
        {

            var buffer = new PacketBuffer();
            buffer.Write(new byte[]{4},0,1);
            buffer.Write(info.ID,0,4);
            return buffer;
        }

        private static IDInfo IDInfoDecode(PacketBuffer buffer)
        {
            return buffer.AvailableBytes() < 4 ? null : new IDInfo { ID = buffer.ReadBytes(4) };
        }
    }
}
