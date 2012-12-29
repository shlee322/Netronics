using System;

namespace Netronics.Scheduling.Microthreading
{
    class SleepData
    {
        public Microthread Thread;
        private readonly long _time;

        public SleepData(Microthread thread, long sec)
        {
            Thread = thread;
            _time = DateTime.Now.Ticks + sec*10000000;
        }

        public bool Pass()
        {
            return DateTime.Now.Ticks >= _time;
        }
    }
}
