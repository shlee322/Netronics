using NUnit.Framework;
using Netronics;
using Netronics.Channel.Channel;

namespace Framework
{
    [TestFixture]
    public class NetronicsTest
    {
        [Test]
        public void Test1()
        {
            var netronics = new Netronics.Netronics(new Properties());
            netronics.Start();
            netronics.AddChannel(SocketChannel.CreateChannel(null));
            netronics.Stop();
        }
    }
}
