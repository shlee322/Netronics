using System.Net;
using Netronics.Channel;
using Netronics;

namespace EchoServer
{
    class Program
    {
        static void Main(string[] args)
        {
            var properties = new Properties();
            properties.SetIpEndPoint(new IPEndPoint(IPAddress.Any, 7777));
            properties.SetChannelFactoryOption(factory => SetFatoryOption((ChannelFactory)factory));

            var netronics = new Netronics.Netronics(properties);
            netronics.Start();
        }

        private static void SetFatoryOption(ChannelFactory factory)
        {
            factory.SetPacketEncoder(() => new PacketEncoder());
            factory.SetPacketDecoder(() => new PacketDecoder());
            factory.SetHandler(() => new Handler());
        }
    }
}