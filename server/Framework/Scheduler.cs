﻿using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading;

namespace Netronics
{
    public class Scheduler
    {
        private static readonly List<Processor> ProcessorList;
        private static readonly ConcurrentQueue<Action> MessageQueue;

        static Scheduler()
        {
            ProcessorList = new List<Processor>();
            MessageQueue = new ConcurrentQueue<Action>();

            SetThreadCount(4);
        }

        public static void SetThreadCount(int count)
        {
            lock (ProcessorList)
            {
                if (count < 0)
                    return;
                if (ProcessorList.Count < count)
                {
                    int newThreadCount = count - ProcessorList.Count;
                    for (int i = 0; i < newThreadCount; i++)
                    {
                        var processor = new Processor();
                        processor.Start();
                        ProcessorList.Add(processor);
                    }
                }
                else if (ProcessorList.Count > count)
                {
                    int removeThreadCount = ProcessorList.Count - count;
                    for (int i = 0; i < removeThreadCount; i++)
                    {
                        if (ProcessorList.Count < 1)
                            break;
                        ProcessorList[ProcessorList.Count - 1].Stop();
                        ProcessorList.RemoveAt(ProcessorList.Count - 1);
                    }
                }
            }
        }


        public static void Add(Action action)
        {
            if (action == null)
                return;
            MessageQueue.Enqueue(action);
        }

        #region Nested type: Processor

        private class Processor
        {
            private bool _run;
            private Thread _thread;

            public void Start()
            {
                _run = true;
                _thread = new Thread(Loop);
                _thread.Start();
            }

            public void Stop()
            {
                _run = false;
            }

            private void Loop()
            {
                while (_run)
                {
                    Thread.Sleep(0);
                    if (MessageQueue.Count == 0)
                        continue;

                    Action action;
                    MessageQueue.TryDequeue(out action);
                    if (action != null)
                        action();
                }
            }
        }

        #endregion
    }
}