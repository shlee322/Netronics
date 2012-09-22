using System.Net;
using System.Threading;
using Netronics;
using Netronics.Channel;

namespace BroadcastServer
{
    class Program
    {
        private static readonly Handler Handler = new Handler();

        static void Main(string[] args)
        {
            // 본 설정은 EchoServer Example와 비슷합니다. 자세한 설명을 보고 싶으시면 EchoServer Example을 참고해주세요.
            var properties = Properties.CreateProperties(new IPEndPoint(IPAddress.Any, 7777),
                                             new ChannelPipe().SetCreateChannelAction(channel => 
                                             {
                                                 channel.SetConfig("encoder", PacketEncoder.Encoder);
                                                 channel.SetConfig("decoder", PacketEncoder.Encoder);
                                                 channel.SetConfig("handler", Handler);
                                             }));

            var netronics = new Netronics.Netronics(properties);
            netronics.Start();
        }
    }
}
