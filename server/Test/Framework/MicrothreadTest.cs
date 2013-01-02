using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using NUnit.Framework;
using Netronics.Scheduling;
using Netronics.Scheduling.Microthreading;
using Netronics.Scheduling.Microthreading.IO;

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
            Scheduler.Default.RunMicrothread(0, new Microthread(() => NPC1(1)));
            Scheduler.Default.RunMicrothread(1, new Microthread(() => NPC1(2)));
            Scheduler.Default.RunMicrothread(0, new Microthread(() => NPC1(3)));
            Scheduler.Default.RunMicrothread(1, new Microthread(() => NPC1(4)));
            _exitEvent.WaitOne();
        }

        [Test]
        public void MicrothreadTest2()
        {
            Scheduler.Default.RunMicrothread(0, new Microthread(NPCTest_1));
            Scheduler.Default.RunMicrothread(0, new Microthread(NPCTest_2));
            _exitEvent.WaitOne();
        }

        [Test]
        public void NIOTest()
        {
            Scheduler.Default.RunMicrothread(0, new Microthread(FileWrite));
            _exitEvent.WaitOne();
        }

        private IEnumerator<IYield> FileWrite()
        {
            byte[] data = Encoding.Default.GetBytes("testtest");
            var file = new FileStream("temp", FileMode.OpenOrCreate);
            yield return file.NioWrite(data, 0, data.Length);
            Console.WriteLine("파일쓰기 완료");
            _exitEvent.Set();
        }

        private WaitEvent _event = new WaitEvent();

        private IEnumerator<IYield> NPCTest_1()
        {
            Console.WriteLine("NPCTest_1 - 1");
            yield return Microthread.Sleep(2);
            Console.WriteLine("NPCTest_1 - 2");
            yield return Microthread.Sleep(2);
            Console.WriteLine("NPCTest_1 - 3");
            _event.Set();
        }

        private IEnumerator<IYield> NPCTest_2()
        {
            Console.WriteLine("NPCTest_2 - 1");
            yield return Microthread.Wait(_event);
            Console.WriteLine("NPCTest_2 - 2");
            _exitEvent.Set();
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
