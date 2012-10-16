using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;

namespace Netronics.Test
{
    [TestFixture]
    class SchedulerTest
    {
        [Test]
        public void SchedulerTest1()
        {
            Scheduler.SetThreadCount(8);
            Scheduler.SetThreadCount(4);
            var random = new Random();
            for (int i = 0; i < 30; i++)
                Scheduler.QueueWorkItem(random.Next(), () => Console.WriteLine("Scheduler Test - Thread ID:" + System.Threading.Thread.CurrentThread.ManagedThreadId));
            
        }
    }
}
