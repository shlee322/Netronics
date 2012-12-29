using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.Concurrent;
using System.Threading;
using Netronics.Scheduling.Microthreading;
using log4net;

namespace Netronics.Scheduling
{
    public class Worker
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(Worker));

        public readonly ConcurrentQueue<Action> _queue;
        private readonly AutoResetEvent _queueEvent = new AutoResetEvent(false);
        public readonly Thread _thread;

        public int _count;
        public bool _run = true;
        public int _time;
        public bool _work;

        public Worker()
        {
            _time = Environment.TickCount;
            _queue = new ConcurrentQueue<Action>();
            _thread = new Thread(Loop);
            _thread.Start();
        }

        /// <summary>
        /// Worker Thread가 Background에서 돌아갈지 설정하는 메소드
        /// </summary>
        /// <param name="background">Background 여부</param>
        public void SetBackground(bool background)
        {
            _thread.IsBackground = background;
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
        /// Microthread를 시작하는 메소드
        /// </summary>
        /// <param name="microthread">시작할 Microthread</param>
        public void RunMicrothread(Microthread microthread)
        {
            QueueWorkItem(() => microthread.Run(this));
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
    }
}
