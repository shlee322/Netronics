using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using NUnit.Framework;
using Netronics.Microthreading;

namespace Netronics.Test
{
    [TestFixture]
    class MicrothreadTest
    {
        private readonly AutoResetEvent _exitEvent = new AutoResetEvent(false);
        private int _i = 4;
        [Test]
        public void MicrothreadTest1()
        {
            Console.WriteLine("Microthread 작동 시작");
            Scheduler.RunMicrothread(0, new Microthread(() => NPC1(1)));
            Scheduler.RunMicrothread(1, new Microthread(() => NPC1(2)));
            Scheduler.RunMicrothread(0, new Microthread(() => NPC1(3)));
            Scheduler.RunMicrothread(1, new Microthread(() => NPC1(4)));
            _exitEvent.WaitOne();
        }

        public IEnumerator<IYield> NPC1(int index)
        {
            Console.WriteLine(DateTime.Now + " NPC" + index + " - 1");
            yield return Microthread.Call(() => NPC1_1(index));
            Console.WriteLine(DateTime.Now + " NPC" + index + " - 4");
            if (Interlocked.Decrement(ref _i) == 0)
                _exitEvent.Set();
            yield return null;
        }

        public IEnumerator<IYield> NPC1_1(int index)
        {
            Console.WriteLine(DateTime.Now + " NPC" + index + " - 2");
            if (index % 2 == 0)
            {
                Console.WriteLine("Sleep");
                yield return Microthread.Sleep(3);
            }
            Console.WriteLine(DateTime.Now + " NPC" + index + " - 3");
        }
    }
}
