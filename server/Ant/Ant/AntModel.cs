using System;
using System.Collections.Generic;
using Netronics.Scheduling.Microthreading;
using Newtonsoft.Json.Linq;

namespace Netronics.Ant.Ant
{
    public class AntModel
    {
        public virtual string GetName()
        {
            throw new NotImplementedException();
        }

        public virtual void OnStart()
        {
        }

        protected void AddTask(int index, Func<Task, IEnumerator<IYield>> task)
        {
            Kernel.GetKernel().AddTask(index, task);
        }

        protected void AddMessage(int index, Func<IAnt, JToken, IEnumerator<IYield>> action)
        {
        }
    }
}
