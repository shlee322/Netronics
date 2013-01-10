using Netronics.Scheduling.Microthreading;
using Newtonsoft.Json.Linq;

namespace Netronics.Ant.Ant.Transaction
{
    class Task
    {
        private WaitEvent _event;

        public Task()
        {
            _event = new WaitEvent(true); //차후 수정할 방법을 생각해보자.
        }

        public WaitEvent GetWaitEvent()
        {
            return _event;
        }

        public void Run(JToken o)
        {
            _event.ForeachMicrothreads((thread) => { thread.Result = o; });
            _event.Set();
        }
    }
}
