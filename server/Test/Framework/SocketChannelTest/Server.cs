using System;
using System.Net;
using Netronics.Channel;
using Netronics.Protocol.PacketEncoder;

namespace Netronics.Test.SocketChannelTest
{
    class Server : IChannelHandler
    {
        private Netronics netronics;

        public void Stop()
        {
            netronics.Stop();
        }

        public Server(IPacketEncoder encoder, IPacketDecoder decoder)
        {
            // Properties를 생성합니다. Properties는 Netronics의 각종 설정을 의미합니다.
            var properties = Properties.CreateProperties(new IPEndPoint(IPAddress.Any, 9999), // 클라이언트를 받을 아이피와 포트를 설정합니다.
                                                         new ChannelPipe().SetCreateChannelAction(channel => // 각 클라이언트가 접속하면 Channel을 생성하는 과정이 시작되는데 옵션을 아래와 같이 넣을 수 있습니다.
                                                         {
                                                             Console.WriteLine("전달하기 전 Channel의 설정");
                                                             channel.SetConfig("encoder", encoder); // 해당 Channel의 Packet Encoder을 설정합니다.
                                                             channel.SetConfig("decoder", decoder); // 해당 Channel의 Packet Decoder을 설정합니다.
                                                             channel.SetConfig("handler", this); // 해당 Channel의 Handler을 설정합니다.

                                                             channel.SetConfig("switch", new RandemSwitch()); //랜덤으로 전달되는지 확인 ReceiveSwitch 테스트

                                                             Console.WriteLine("Channel을 Netronics로 전달");
                                                         }));

            netronics = new Netronics(properties); // 위에서 만든 Properties를 바탕으로 Netronics 객체를 생성합니다.
            netronics.Start(); // 서버를 가동합니다.
        }

        // 클라이언트가 접속시 호출
        public void Connected(IReceiveContext channel)
        {
            //channel.GetChannel().SendMessage(System.Text.Encoding.Default.GetBytes("test"));
        }

        // 클라이언트 접속종료시 호출
        public void Disconnected(IReceiveContext channel)
        {
        }

        // 클라이언트로부터 메시지가 왔을때 호출
        public void MessageReceive(IReceiveContext context)
        {
            if(((string) context.GetMessage()).Substring(0,4) != "test")
                throw new Exception("Error!");
            Console.WriteLine("Server Thread " + System.Threading.Thread.CurrentThread.ManagedThreadId + " - " + context.GetMessage());
            context.GetChannel().SendMessage(context.GetMessage());
            //context.GetChannel().SendMessage(context.GetMessage()); // 에코 서버인 만큼 온 데이터를 그대로 전송
        }
    }
}
