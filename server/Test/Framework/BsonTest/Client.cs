using System;
using System.Net;
using Netronics.Channel;
using Netronics.Protocol.PacketEncoder;
using Netronics.Protocol.PacketEncoder.Bson;
using Newtonsoft.Json.Linq;

namespace Netronics.Test.BsonTest
{
    class Client : IChannelHandler
    {
        private global::Netronics.Client netronics;
        private int i = 0;

        public void Stop()
        {
            netronics.Stop();
        }

        public Client()
        {
            // Properties를 생성합니다. Properties는 Netronics의 각종 설정을 의미합니다.
            var properties = Properties.CreateProperties(new IPEndPoint(IPAddress.Loopback, 9999), // 서버의 아이피와 포트를 설정합니다.
                                                         new ChannelPipe().SetCreateChannelAction(channel =>
                                                         {
                                                             channel.SetConfig("encoder", BsonEncoder.Encoder); // 해당 Channel의 Packet Encoder을 설정합니다.
                                                             channel.SetConfig("decoder", BsonDecoder.Decoder); // 해당 Channel의 Packet Decoder을 설정합니다.
                                                             channel.SetConfig("handler", this); // 해당 Channel의 Handler을 설정합니다.

                                                             //기본적인 switch는 같은 채널은 같은 스래드에서 처리되기때문에 메시지 처리가 같은 스레드에서 일어날것.
                                                         }));

            netronics = new global::Netronics.Client(properties); // 위에서 만든 Properties를 바탕으로 Netronics 객체를 생성합니다.
            netronics.Start(); // 서버를 가동합니다.
        }

        // 클라이언트가 접속시 호출
        public void Connected(IReceiveContext channel)
        {
            JObject o = new JObject();
            o.Add("name", "test");
            o.Add("value", i++);
            channel.GetChannel().SendMessage(o);
        }

        // 클라이언트 접속종료시 호출
        public void Disconnected(IReceiveContext channel)
        {
        }

        // 클라이언트로부터 메시지가 왔을때 호출
        public void MessageReceive(IReceiveContext context)
        {
            var o = context.GetMessage() as JObject;

            if (o.Value<string>("name") != "test")
                throw new Exception("Error!");

            JObject o2 = new JObject();
            o2.Add("name", "test");
            o2.Add("value", i++);
            context.GetChannel().SendMessage(o2);

            Console.WriteLine("Client Thread " + System.Threading.Thread.CurrentThread.ManagedThreadId + " - " + context.GetMessage());
            if (((JObject) context.GetMessage()).Value<int>("value") == 20)
                SocketChannel.ExitEvent.Set();
        }
    }
}
