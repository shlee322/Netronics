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
        private readonly Microthread _root;
        private readonly Func<IEnumerator<IYield>> _func;
        private IEnumerator<IYield> _enumerator;
        
        private bool _wait = false;

        private object _result;
        private object _tag;

        static Microthread()
        {
            SleepThread = new Thread(SleepThreadLoop);
            SleepThread.Start();
        }

        public Microthread(Func<IEnumerator<IYield>> func, Microthread parent = null)
        {
            _func = func;
            if (parent == null)
            {
                _root = this;
            }
            else
            {
                _parent = parent;
                _root = _parent._root;
            }
        }

        /// <summary>
        /// 프레임워크 내부적으로 사용되는 메서드
        /// </summary>
        /// <param name="scheduler"></param>
        public void Run(Scheduler scheduler)
        {
            _scheduler = scheduler;
            Thread.Value = this;
            if (_enumerator == null)
                _enumerator = _func();

            bool end = Loop(scheduler);

            Thread.Value = null;

            if (_parent != null && end)
                Run(_parent);
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

        public static IYield Wait(WaitEvent waitEvent)
        {
            if (waitEvent.IsSet())
            {
                Run(Thread.Value);
                return null;
            }

            waitEvent.AddWaitMicrothread(Thread.Value);
            return new WaitEventYield();
        }

        public static void Run(Microthread microthread)
        {
            microthread._scheduler.RunMicrothread(microthread);
        }

        public static Microthread CurrentMicrothread
        {
            get { return Thread.Value; }
        }

        public object Result
        {
            get { return _root._result; }
            set { _root._result = value; }
        }

        public object Tag
        {
            get { return _root._tag; }
            set { _root._tag = value; }
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
                    Run(sleepData.Thread);
                }

                System.Threading.Thread.Sleep(250);
            }
        }
    }
}
