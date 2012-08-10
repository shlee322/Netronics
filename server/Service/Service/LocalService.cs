using System;
using System.Collections.Generic;
using Service.Service.Task;

namespace Service.Service
{
    public class LocalService : Service
    {
        public virtual void Load(Manager.ManagerProcessor processor)
        {
        }

        public virtual void Start()
        {
        }

        public virtual void Stop()
        {
        }

        public virtual void MessageReceive(Service service, object o)
        {
        }

        public void Call(long uid, Func<IEnumerator<Task.Task>> func)
        {
            GetManagerProcessor().Call(uid, func);
        }
    }
}
