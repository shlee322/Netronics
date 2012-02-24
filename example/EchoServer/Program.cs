using System.Net;
using Netronics.Channel;
using Netronics;
using Netronics.Protocol;

namespace EchoServer
{
    class Program
    {
        static void Main(string[] args)
        {
            Scheduler.SetThreadCount(4);
            var properties = new Properties();
            properties.SetIpEndPoint(new IPEndPoint(IPAddress.Any, 7777));
            properties.SetChannelFactoryOption(factory => SetFatoryOption((ChannelFactory)factory));

            var netronics = new Netronics.Netronics(properties);
            netronics.Start();
        }

        private static void SetFatoryOption(ChannelFactory factory)
        {
            factory.SetProtocol(() => new ModifiableProtocol(encoder: new PacketEncoder(), decoder: new PacketDecoder()));
            factory.SetHandler(() => new Handler());
        }
    }
}