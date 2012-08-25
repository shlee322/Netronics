using System.Collections.Generic;
using System.Threading;
using NUnit.Framework;
using Netronics.Microthreading;

namespace Framework
{
    [TestFixture]
    class MicrothreadTest
    {
        [Test]
        public void MicrothreadTest1()
        {
            var ExitEvent = new AutoResetEvent(false);
            Netronics.Scheduler.RunMicrothread(0, new Microthread(()=>NPC1(1)));
            Netronics.Scheduler.RunMicrothread(1, new Microthread(() => NPC1(2)));
            Netronics.Scheduler.RunMicrothread(0, new Microthread(() => NPC1(3)));
            Netronics.Scheduler.RunMicrothread(1, new Microthread(() => NPC1(4)));
            ExitEvent.WaitOne();
        }

        public IEnumerator<IYield> NPC1(int index)
        {
            System.Console.WriteLine("NPC" + index + " - 1");
            yield return Microthread.Call(()=>NPC1_1(index));
            System.Console.WriteLine("NPC" + index + " - 4");
            yield return null;
        }

        public IEnumerator<IYield> NPC1_1(int index)
        {
            System.Console.WriteLine("NPC" + index + " - 2");
            if(index % 2 == 0)
                yield return Microthread.Sleep(3);
            System.Console.WriteLine("NPC" + index + " - 3");
        }
    }
}
