using System;
using Netronics.Channel;

namespace Netronics.Test.BsonTest
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
