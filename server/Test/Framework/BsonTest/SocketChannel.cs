using System.Threading;
using NUnit.Framework;
using Netronics.Protocol.PacketEncoder.Linefeed;

namespace Netronics.Test.BsonTest
{
    [TestFixture]
    class SocketChannel
    {
        public static readonly AutoResetEvent ExitEvent = new AutoResetEvent(false);

        [Test]
        public void SocketChannelTest1()
        {
            System.Console.WriteLine("Bson Test");

            var server = new Server();
            var client = new Client();
            ExitEvent.WaitOne();
            client.Stop();
            server.Stop();
        }
    }
}
