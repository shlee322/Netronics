using Netronics.Scheduling.Microthreading;
using Newtonsoft.Json.Linq;

namespace Netronics.Ant.Ant.Transaction
{
    class Task
    {
        private WaitEvent _event;

        public Task()
        {
            _event = new WaitEvent();
        }

        public WaitEvent GetWaitEvent()
        {
            return _event;
        }

        public void Run(JToken o)
        {
            foreach (var thread in _event.GetWaitMicrothread())
            {
                thread.Result = o;
            }
            _event.Set();
        }

    }
}
