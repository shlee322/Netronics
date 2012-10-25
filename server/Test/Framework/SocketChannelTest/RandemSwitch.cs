using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Netronics.Channel;

namespace Netronics.Test.SocketChannelTest
{
    class RandemSwitch : IReceiveSwitch
    {
        private static readonly Random Random = new Random();

        public int ReceiveSwitching(IReceiveContext context)
        {
            return Random.Next(3);
        }
    }
}
