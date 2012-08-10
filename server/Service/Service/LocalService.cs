using System;
using System.Collections.Generic;
using Service.Service.Task;

namespace Service.Service
{
    public class LocalService : Service
    {
        private Manager.ManagerProcessor _processor;

        public void SetManagerProcessor(Manager.ManagerProcessor processor)
        {
            _processor = processor;
        }

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
            _processor.Call(uid, func);
        }
    }
}
