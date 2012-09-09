using System;
using System.Collections.Concurrent;
using System.Diagnostics;
using System.Threading;
using log4net;

namespace Netronics
{
    public class Scheduler
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(Scheduler)); 

        private static Scheduler[] _schedulers = new Scheduler[0];
        private static Thread _monitoringThread;

        private bool _run = true;
        private readonly Thread _thread;

        private readonly ConcurrentQueue<Action> _queue;
        private readonly AutoResetEvent _queueEvent = new AutoResetEvent(false);

        private int _time;
        private int _count;
        private bool _work;

        static Scheduler()
        {
            SetThreadCount(4);
        }

        private Scheduler(bool background)
        {
            _queue = new ConcurrentQueue<Action>();
            _thread = new Thread(Loop);
            _thread.IsBackground = background;
            _thread.Start();
        }

        public static void SetThreadCount(int count, bool background = false)
        {
            if (count < 1)
                return;

            try
            {
                if(_monitoringThread != null)
                    _monitoringThread.Abort();
            }
            catch (ThreadStateException)
            {
            }
            
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
                    temp[i] = new Scheduler(background);
                _schedulers = temp;
            }

            _monitoringThread = new Thread(Monitoring);
            _monitoringThread.Start();
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
            _time = Environment.TickCount;
            while (_run)
            {
                _queue.TryDequeue(out action);
                if (action == null)
                {
                    _queueEvent.WaitOne();
                    continue;
                }

                _work = true;
                try
                {
                    action();
                }
                catch (Exception e)
                {
                    Log.Error("Netronics Scheduler Action Error", e);
                }

                _time = Environment.TickCount;
                _count++;
                _work = false;
            }
        }

        private static void Monitoring()
        {
            while (true)
            {
                int time = Environment.TickCount;
                for (int i = 0; i < _schedulers.Length; i++)
                {
                    var scheduler = _schedulers[i];
                    Log.InfoFormat("Thread ID : {0}, Queue : {1}, Last : {2}ms, TPS : {3} ({4:N})", i, scheduler._queue.Count, time - scheduler._time, scheduler._count, scheduler._count / 60);
                    
                    //1분 이상 지연됨
                    if (time - scheduler._time > 60000 && scheduler._work)
                    {
                        try
                        {
                            scheduler._thread.Suspend();
                            var stack = new StackTrace(scheduler._thread, true);
                            Log.InfoFormat("Thread ID : {0}, Stack {1}", i, stack);
                            scheduler._thread.Resume();
                        }
                        catch (Exception e)
                        {
                            Log.Error("Scheduler Error", e);
                        }
                    }
                    scheduler._count = 0;
                }
                
                Thread.Sleep(60000);
            }
        }
    }
}
