using System;
using System.Collections.Concurrent;
using System.Diagnostics;
using System.Threading;
using Netronics.Scheduling.Microthreading;
using log4net;

namespace Netronics.Scheduling
{
    /// <summary>
    /// Netronics의 Worker Thread를 관리 해주는 스케줄러 클래스
    /// </summary>
    public class Scheduler
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof (Scheduler));
        public static readonly Scheduler Default = new Scheduler();

        private Worker[] _workers = new Worker[0];
        private Thread _monitoringThread;

        public Scheduler(int count = 4)
        {
            SetThreadCount(count);
        }

        /// <summary>
        /// Worker Therad의 갯수와 Background 여부를 지정하는 메소드
        /// 단, SetThreadCount 메소드는 Netronics가 시작되기전에 호출하여야 함
        /// </summary>
        /// <param name="count">Thread 갯수</param>
        /// <param name="background">Background 여부</param>
        public void SetThreadCount(int count, bool background = false)
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


            if (_workers.Length > count) //기존 Worker Thread 수보다 설정할 갯수가 적으면 삭제
            {
                for (int i = _workers.Length - 1; i >= count; i--)
                {
                    _workers[i]._run = false;
                    _workers[i] = null;
                }

                var temp = new Worker[count];

                Array.Copy(_workers, temp, count);

                _workers = temp;
            }
            else if (_workers.Length < count) //기존 Worker Thread 수보다 설정할 갯수가 많으면 추가
            {
                var temp = new Worker[count];

                Array.Copy(_workers, temp, _workers.Length);

                for (int i = _workers.Length; i < temp.Length; i++)
                    temp[i] = new Worker();

                _workers = temp;
            }

            foreach (Worker worker in _workers)
            {
                worker.SetBackground(background);
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
        public int GetThreadCount()
        {
            return _workers.Length;
        }

        /// <summary>
        /// 해당 index에 맞는 Worker Thread의 작업 큐의 맨 끝에 Action을 추가하는 메소드
        /// 같은 index를 가지는 Action은 모두 같은 스래드에서 처리된다.
        /// </summary>
        /// <param name="index">작업구분을 위한 index</param>
        /// <param name="action">호출할 Action</param>
        public void QueueWorkItem(int index, Action action)
        {
            _workers[index % _workers.Length].QueueWorkItem(action);
        }

        /// <summary>
        /// 해당 index에 맞는 Worker Thread에서 Microthread를 시작하는 메소드
        /// </summary>
        /// <param name="index">작업구분을 위한 index</param>
        /// <param name="microthread">시작할 Microthread</param>
        public void RunMicrothread(int index, Microthread microthread)
        {
            _workers[index % _workers.Length].RunMicrothread(microthread);
        }

        public void SetDefaultTimeout(int warning=1000, int timeout=-1)
        {
        }

        public void SetCurrentTimeout(int time, Action action)
        {
        }

        /// <summary>
        /// Worker Thread를 모니터링 하는 Thread의 메인 Loop
        /// </summary>
        private void Monitoring()
        {
            while (true)
            {
                int time = Environment.TickCount;
                for (int i = 0; i < _workers.Length; i++)
                {
                    Worker worker = _workers[i];
                    Log.InfoFormat("Thread ID : {0}, Queue : {1}, Last : {2}ms, TPS : {3} ({4:N})", i,
                                   worker._queue.Count, time - worker._time, worker._count,
                                   worker._count / 60.0f);

                    //1분 이상 지연됨
                    if (time - worker._time > 60000 && worker._work)
                    {
                        try
                        {
                            worker._thread.Suspend();
                            var stack = new StackTrace(worker._thread, true);
                            Log.InfoFormat("Thread ID : {0}, Stack {1}", i, stack);
                            worker._thread.Resume();
                        }
                        catch (Exception e)
                        {
                            Log.Error("Scheduler Error", e);
                        }
                    }
                    worker._count = 0;
                }

                Thread.Sleep(60000);

            }
        }
    }
}