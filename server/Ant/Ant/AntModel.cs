using System;
using System.Collections.Generic;
using Netronics.Scheduling.Microthreading;

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
            throw new NotImplementedException();
        }

        protected void AddTask(int index, Func<IAnt, object, IEnumerator<IYield>> task)
        {
        }

        protected void AddMessage(int index, Func<IAnt, object, IEnumerator<IYield>> action)
        {
        }
    }
}
