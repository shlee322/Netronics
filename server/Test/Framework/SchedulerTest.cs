using NUnit.Framework;

namespace Framework
{
    [TestFixture]
    class SchedulerTest
    {
        [Test]
        public void SchedulerTest1()
        {
            Netronics.Scheduler.SetThreadCount(4);
            Netronics.Scheduler.SetThreadCount(0);
        }
    }
}
