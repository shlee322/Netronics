using System;
using System.Collections.Concurrent;
using System.Diagnostics;
using System.Threading;
using Netronics.Microthreading;
using log4net;

namespace Netronics
{
    /// <summary>
    /// Netronics의 Worker Thread를 관리 해주는 스케줄러 클래스
    /// </summary>
    public class Scheduler
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof (Scheduler));

        private static Scheduler[] _schedulers = new Scheduler[0];
        private static Thread _monitoringThread;

        private readonly ConcurrentQueue<Action> _queue;
        private readonly AutoResetEvent _queueEvent = new AutoResetEvent(false);
        private readonly Thread _thread;

        private int _count;
        private bool _run = true;
        private int _time;
        private bool _work;

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

        /// <summary>
        /// Worker Thread가 Background에서 돌아갈지 설정하는 메소드
        /// </summary>
        /// <param name="background">Background 여부</param>
        private void SetBackground(bool background)
        {
            _thread.IsBackground = background;
        }

        /// <summary>
        /// Worker Therad의 갯수와 Background 여부를 지정하는 메소드
        /// 단, SetThreadCount 메소드는 Netronics가 시작되기전에 호출하여야 함
        /// </summary>
        /// <param name="count">Thread 갯수</param>
        /// <param name="background">Background 여부</param>
        public static void SetThreadCount(int count, bool background = false)
        {
            if (count < 1)
                return;

            //모니터링 스래드가 존재하면 종료
            try
            {
                if (_monitoringThread != null)
                    _monitoringThread.Abort();
                _monitoringThread = null;
            }
            catch (ThreadStateException)
            {
            }


            if (_schedulers.Length > count) //기존 Worker Thread 수보다 설정할 갯수가 적으면 삭제
            {
                for (int i = _schedulers.Length - 1; i >= count; i--)
                {
                    _schedulers[i]._run = false;
                    _schedulers[i] = null;
                }

                var temp = new Scheduler[count];

                Array.Copy(_schedulers, temp, count);

                _schedulers = temp;
            }
            else if (_schedulers.Length < count) //기존 Worker Thread 수보다 설정할 갯수가 많으면 추가
            {
                var temp = new Scheduler[count];

                Array.Copy(_schedulers, temp, _schedulers.Length);

                for (int i = _schedulers.Length; i < temp.Length; i++)
                    temp[i] = new Scheduler();

                _schedulers = temp;
            }

            foreach (Scheduler scheduler in _schedulers)
            {
                scheduler.SetBackground(background);
            }

            if (_monitoringThread == null)
            {
                _monitoringThread = new Thread(Monitoring);
                _monitoringThread.Start();
            }
        }

        /// <summary>
        /// SetThreadCount 메소드로 설정한 Thread의 갯수를 반환하는 메소드
        /// </summary>
        /// <returns>Thread의 갯수</returns>
        public static int GetThreadCount()
        {
            return _schedulers.Length;
        }

        /// <summary>
        /// Worker Thread의 작업 큐의 맨 끝에 Action을 추가하는 메소드
        /// </summary>
        /// <param name="action">호출할 Action</param>
        public void QueueWorkItem(Action action)
        {
            _queue.Enqueue(action);
            _queueEvent.Set();
        }

        /// <summary>
        /// 해당 index에 맞는 Worker Thread의 작업 큐의 맨 끝에 Action을 추가하는 메소드
        /// 같은 index를 가지는 Action은 모두 같은 스래드에서 처리된다.
        /// </summary>
        /// <param name="index">작업구분을 위한 index</param>
        /// <param name="action">호출할 Action</param>
        public static void QueueWorkItem(int index, Action action)
        {
            _schedulers[index%_schedulers.Length].QueueWorkItem(action);
        }

        /// <summary>
        /// Microthread를 시작하는 메소드
        /// </summary>
        /// <param name="microthread">시작할 Microthread</param>
        public void RunMicrothread(Microthread microthread)
        {
            QueueWorkItem(() => microthread.Run(this));
        }

        /// <summary>
        /// 해당 index에 맞는 Worker Thread에서 Microthread를 시작하는 메소드
        /// </summary>
        /// <param name="index">작업구분을 위한 index</param>
        /// <param name="microthread">시작할 Microthread</param>
        public static void RunMicrothread(int index, Microthread microthread)
        {
            _schedulers[index%_schedulers.Length].RunMicrothread(microthread);
        }

        public static void SetDefaultTimeout(int warning=1000, int timeout=-1)
        {
        }

        public static void SetCurrentTimeout(int time, Action action)
        {
        }

        /// <summary>
        /// Worker Thread의 메인 Loop 메소드
        /// </summary>
        private void Loop()
        {
            Action action;
            _time = Environment.TickCount;
            while (_run)
            {
                _queue.TryDequeue(out action);

                if (action == null) //큐에 Action이 없으면 큐에 Action이 추가 될때까지 대기
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

        /// <summary>
        /// Worker Thread를 모니터링 하는 Thread의 메인 Loop
        /// </summary>
        private static void Monitoring()
        {
            while (true)
            {
                int time = Environment.TickCount;
                for (int i = 0; i < _schedulers.Length; i++)
                {
                    Scheduler scheduler = _schedulers[i];
                    Log.InfoFormat("Thread ID : {0}, Queue : {1}, Last : {2}ms, TPS : {3} ({4:N})", i,
                                   scheduler._queue.Count, time - scheduler._time, scheduler._count,
                                   scheduler._count/60.0f);

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