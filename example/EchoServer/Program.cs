using System.Net;
using Netronics.Channel;
using Netronics;

namespace EchoServer
{
    class Program
    {
        static void Main(string[] args)
        {
            new Netronics.Netronics(
                new Properties().SetIpEndPoint(new IPEndPoint(IPAddress.Any, 7777)).SetChannelFactoryOption(factory => {
                    var basicChannelFactory = (BasicChannelFactory)factory;
                        basicChannelFactory.SetPacketEncoder(() => new PacketEncoder())
                                           .SetPacketDecoder(() => new PacketDecoder())
                                           .SetHandler(() => new Handler());
                })).Start();
        }
    }
}
