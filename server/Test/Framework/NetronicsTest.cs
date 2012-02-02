using NUnit.Framework;

namespace Framework
{
    [TestFixture]
    public class NetronicsTest
    {
        [Test]
        public void Test1()
        {
            var netronics = new Netronics.Netronics(null);
            netronics.Start();
            netronics.AddChannel(Netronics.Channel.Channel.CreateChannel(null, null, null, null));
            netronics.Stop();
        }
    }
}
