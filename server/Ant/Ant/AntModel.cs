using System;
using System.Collections.Generic;
using Netronics.Microthreading;

namespace Netronics.Ant.Ant
{
    public class AntModel : IAnt
    {
        protected void AddTask(int index, Func<IAnt, object, IEnumerator<IYield>> task)
        {
        }

        protected void AddMessage(int index, Func<IAnt, object, IEnumerator<IYield>> action)
        {
        }

        public virtual string GetName()
        {
            throw new NotImplementedException();
        }

        public IYield SendTask(int index)
        {
            return null;
        }

        public IYield SendMessage(int index)
        {
            return null;
        }
    }
}
