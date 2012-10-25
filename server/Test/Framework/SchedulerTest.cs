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
            Console.WriteLine("쓰레드 설정 테스트 시작");
            Scheduler.SetThreadCount(8);
            Scheduler.SetThreadCount(4);
            Console.WriteLine("쓰레드 설정 테스트 완료");

            Console.WriteLine("작업 등록 테스트 시작");
            var random = new Random();
            for (int i = 0; i < 30; i++)
                Scheduler.QueueWorkItem(random.Next(), () => Console.WriteLine("[작업처리] Scheduler Test - Thread ID:" + System.Threading.Thread.CurrentThread.ManagedThreadId));
            Console.WriteLine("작업 등록 테스트 끝");
        }
    }
}
