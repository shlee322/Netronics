using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Netronics
{
    class Scheduler
    {
        private static List<Thread> _threadList; 
        private static ConcurrentQueue<Action> _messageQueue;

        static Scheduler()
        {
            _threadList = new List<Thread>();
            _messageQueue = new ConcurrentQueue<Action>();
        }

        public static void SetThreadCount(int count)
        {
            lock (_threadList)
            {
                if (count < 0)
                    return;
                if(_threadList.Count < count)
                {
                    int newThreadCount = count - _threadList.Count;
                    for (int i = 0; i < newThreadCount; i++)
                    {
                        var thread = new Thread(Loop);
                        thread.Start();
                        _threadList.Add(thread);
                    }
                }else if(_threadList.Count > count)
                {
                    int removeThreadCount = _threadList.Count - count;
                    for (int i = 0; i < removeThreadCount; i++)
                    {
                        if(_threadList.Count < 1)
                            break;
                        _threadList[_threadList.Count - 1].Abort();
                    }
                }
            }
        }

        private static void Loop()
        {
            try
            {
                while(true)
                {
                }
            }
            catch (ThreadAbortException)
            {
            }
        }

        public static void Add(Action action)
        {
            if (action == null)
                return;
        }
    }
}
