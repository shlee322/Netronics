using System;
using System.Collections.Concurrent;
using System.Threading;
using log4net;

namespace Netronics
{
    public class Scheduler
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(Scheduler)); 

        private static Scheduler[] _schedulers = new Scheduler[0];

        private bool _run = true;
        private readonly Thread _thread;

        private readonly ConcurrentQueue<Action> _queue;
        private readonly AutoResetEvent _queueEvent = new AutoResetEvent(false);

        static Scheduler()
        {
            SetThreadCount(4);
        }

        private Scheduler()
        {
            _queue = new ConcurrentQueue<Action>();
            _thread = new Thread(Loop);
            _thread.Start();
        }

        public static void SetThreadCount(int count)
        {
            if (count < 1)
                return;

            if (_schedulers.Length > count)
            {
                for (int i = _schedulers.Length - 1; i >= count; i--)
                {
                    _schedulers[i]._run = false;
                    _schedulers[i] = null;
                }
            }
            else if (_schedulers.Length < count)
            {
                var temp = new Scheduler[count];
                for (int i = 0; i < _schedulers.Length; i++)
                    temp[i] = _schedulers[i];
                for (int i = _schedulers.Length; i < temp.Length; i++)
                    temp[i] = new Scheduler();
                _schedulers = temp;
            }
        }

        public static int GetThreadCount()
        {
            return _schedulers.Length;
        }

        public void QueueWorkItem(Action action)
        {
            _queue.Enqueue(action);
            _queueEvent.Set();
        }

        public static void QueueWorkItem(int index, Action action)
        {
            _schedulers[index % _schedulers.Length].QueueWorkItem(action);
        }

        public void RunMicrothread(Microthreading.Microthread microthread)
        {
            QueueWorkItem(()=>microthread.Run(this));
        }

        public static void RunMicrothread(int index, Microthreading.Microthread microthread)
        {
            _schedulers[index % _schedulers.Length].RunMicrothread(microthread);
        }

        private void Loop()
        {
            Action action;
            while (_run)
            {
                _queue.TryDequeue(out action);
                if (action == null)
                {
                    _queueEvent.WaitOne();
                    continue;
                }

                try
                {
                    action();
                }
                catch (Exception e)
                {
                    Log.Error("Netronics Scheduler Action Error", e);
                }
            }
        }
    }
}
