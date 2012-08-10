using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading;
namespace Service.Service.Task
{
    public class Scheduler
    {
        private readonly Thread _thread;
        private ConcurrentQueue<Task> _queue = new ConcurrentQueue<Task>();

        public Scheduler()
        {
            _thread = new Thread(Run);
            _thread.Start();
        }

        private void Run()
        {
            while (true)
            {
                Task task;
                _queue.TryDequeue(out task);
                if(task != null)
                    task.Call(this);
                Thread.Sleep(0);
            }
        }

        public void Add(Task task)
        {
            _queue.Enqueue(task);
        }
    }
}
