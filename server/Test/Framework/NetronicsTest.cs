﻿using NUnit.Framework;
using Netronics;

namespace Framework
{
    [TestFixture]
    public class NetronicsTest
    {
        [Test]
        public void Test1()
        {
            Scheduler.SetThreadCount(4);
            var netronics = new Netronics.Netronics(new Properties());
            netronics.Start();
            netronics.AddChannel(Netronics.Channel.SocketChannel.CreateChannel(null));
            netronics.Stop();
            Scheduler.SetThreadCount(0);
        }
    }
}
