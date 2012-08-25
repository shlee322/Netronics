using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading;

namespace Netronics.Microthreading
{
    public class Microthread : IYield
    {
        private static readonly ThreadLocal<Microthread> Thread = new ThreadLocal<Microthread>();
        private static readonly Thread SleepThread;
        private static readonly ConcurrentQueue<SleepData> SleepDataQueue = new ConcurrentQueue<SleepData>(); 
        private static readonly LinkedList<SleepData> SleepDatas = new LinkedList<SleepData>();


        private Scheduler _scheduler;
        private readonly Microthread _parent;
        private readonly Func<IEnumerator<IYield>> _func;
        private IEnumerator<IYield> _enumerator;

        private bool _wait = false;

        static Microthread()
        {
            SleepThread = new Thread(SleepThreadLoop);
            SleepThread.Start();
        }

        public Microthread(Func<IEnumerator<IYield>> func, Microthread parent = null)
        {
            _func = func;
            _parent = parent;
        }

        public void Run(Scheduler scheduler)
        {
            _scheduler = scheduler;
            Thread.Value = this;
            if (_enumerator == null)
                _enumerator = _func();

            bool end = Loop(scheduler);

            Thread.Value = null;

            if (_parent != null && end)
                scheduler.RunMicrothread(_parent);
        }

        private bool Loop(Scheduler scheduler)
        {
            if(_enumerator.MoveNext())
            {
                if (_enumerator.Current is Microthread)
                    scheduler.RunMicrothread(_enumerator.Current as Microthread);
                return false;
            }
            return true;
        }

        public static IYield Call(Func<IEnumerator<IYield>> func)
        {
            return new Microthread(func, Thread.Value);
        }

        public static IYield Sleep(long sec)
        {
            if(sec < 1)
                throw new Exception("시간은 1이상이여야 합니다.");
            SleepDataQueue.Enqueue(new SleepData(Thread.Value, sec));
            return new SleepYield();
        }

        private static void SleepThreadLoop()
        {
            while (true)
            {
                SleepData data;
                while (SleepDataQueue.TryDequeue(out data))
                    SleepDatas.AddLast(data);

                var removeData = new LinkedList<SleepData>();
                foreach (var sleepData in SleepDatas)
                {
                    if(sleepData.Pass())
                        removeData.AddLast(sleepData);
                }
                foreach (var sleepData in removeData)
                {
                    SleepDatas.Remove(sleepData);
                    sleepData.Thread._scheduler.RunMicrothread(sleepData.Thread);
                }

                System.Threading.Thread.Sleep(250);
            }
        }
    }
}
