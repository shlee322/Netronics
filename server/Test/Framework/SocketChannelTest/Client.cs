using System;
using System.Net;
using System.Text;
using Netronics.Channel;
using Netronics.Protocol.PacketEncoder;

namespace Netronics.Test.SocketChannelTest
{
    class Client : IChannelHandler
    {
        private global::Netronics.Client netronics;
        private int i = 0;

        public void Stop()
        {
            netronics.Stop();
        }

        public Client(IPacketEncoder encoder, IPacketDecoder decoder)
        {
            // Properties를 생성합니다. Properties는 Netronics의 각종 설정을 의미합니다.
            var properties = Properties.CreateProperties(new IPEndPoint(IPAddress.Loopback, 9999), // 서버의 아이피와 포트를 설정합니다.
                                                         new ChannelPipe().SetCreateChannelAction(channel =>
                                                         {
                                                             channel.SetConfig("encoder", encoder); // 해당 Channel의 Packet Encoder을 설정합니다.
                                                             channel.SetConfig("decoder", decoder); // 해당 Channel의 Packet Decoder을 설정합니다.
                                                             channel.SetConfig("handler", this); // 해당 Channel의 Handler을 설정합니다.

                                                             //기본적인 switch는 같은 채널은 같은 스래드에서 처리되기때문에 메시지 처리가 같은 스레드에서 일어날것.
                                                         }));

            netronics = new global::Netronics.Client(properties); // 위에서 만든 Properties를 바탕으로 Netronics 객체를 생성합니다.
            netronics.Start(); // 서버를 가동합니다.
        }

        // 클라이언트가 접속시 호출
        public void Connected(IReceiveContext channel)
        {
            channel.GetChannel().SendMessage("test" + i++);
        }

        // 클라이언트 접속종료시 호출
        public void Disconnected(IReceiveContext channel)
        {
        }

        // 클라이언트로부터 메시지가 왔을때 호출
        public void MessageReceive(IReceiveContext context)
        {
            context.GetChannel().SendMessage("test" + i++);
            Console.WriteLine("Client Thread " + System.Threading.Thread.CurrentThread.ManagedThreadId + " - " + context.GetMessage());
            if ((string) context.GetMessage() == "test20")
                SocketChannel.ExitEvent.Set();
        }
    }
}
