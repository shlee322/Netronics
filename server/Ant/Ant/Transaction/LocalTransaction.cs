using Netronics.Scheduling.Microthreading;
using Newtonsoft.Json.Linq;

namespace Netronics.Ant.Ant.Transaction
{
    class LocalTransaction : Transaction
    {
        private LocalAnt _ant;

        public LocalTransaction(LocalAnt ant)
        {
            _ant = ant;
        }

        protected override void SendTask2(int tid, int type, JToken o)
        {
            var taskObj = new Ant.Task(_ant, tid, type, o);
            var task = Kernel.GetKernel().GetTask(type);
            Scheduling.Scheduler.Default.RunMicrothread(o.GetHashCode(), new Microthread(() => task(taskObj)));
        }
    }
}
