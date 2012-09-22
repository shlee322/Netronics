using System.Net;
using System.Threading;
using Netronics.Channel;
using Netronics;
using Netronics.Protocol;

namespace EchoServer
{
    class Program
    {
        static void Main(string[] args)
        {
            // Properties를 생성합니다. Properties는 Netronics의 각종 설정을 의미합니다.
            var properties = Properties.CreateProperties(new IPEndPoint(IPAddress.Any, 7777), // 클라이언트를 받을 아이피와 포트를 설정합니다.
                                                         new ChannelPipe().SetCreateChannelAction(channel => // 각 클라이언트가 접속하면 Channel을 생성하는 과정이 시작되는데 옵션을 아래와 같이 넣을 수 있습니다.
                                                             {
                                                                 channel.SetConfig("encoder", PacketEncoder.Encoder); // 해당 Channel의 Packet Encoder을 설정합니다.
                                                                 channel.SetConfig("decoder", PacketEncoder.Encoder); // 해당 Channel의 Packet Decoder을 설정합니다.
                                                                 channel.SetConfig("handler", new Handler()); // 해당 Channel의 Handler을 설정합니다.
                                                             }));

            var netronics = new Netronics.Netronics(properties); // 위에서 만든 Properties를 바탕으로 Netronics 객체를 생성합니다.
            netronics.Start(); // 서버를 가동합니다.
        }
    }
}