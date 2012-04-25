using System.Net;
using System.Threading;
using Netronics.Channel;
using Netronics;
using Netronics.Protocol;

namespace EchoServer
{
    class Program
    {
        private static readonly AutoResetEvent ExitEvent = new AutoResetEvent(false);

        static void Main(string[] args)
        {
            var properties = new Properties();
            properties.SetIpEndPoint(new IPEndPoint(IPAddress.Any, 7777));
            properties.SetChannelFactoryOption(factory => SetFactoryOption((ChannelFactory)factory));

            var netronics = new Netronics.Netronics(properties);
            netronics.Start();

            ExitEvent.WaitOne();
        }

        private static void SetFactoryOption(ChannelFactory factory)
        {
            factory.SetProtocol(() => new ModifiableProtocol(encoder: new PacketEncoder(), decoder: new PacketDecoder()));
            factory.SetHandler(() => new Handler());
        }
    }
}