using System;
using System.Collections.Generic;
using System.Threading;

namespace Service.Service.Task
{
    public class Task
    {
        private Func<IEnumerator<Task>> _func;
        private IEnumerator<Task> _enumerator;
        private static readonly ThreadLocal<Task> CurrentTask = new ThreadLocal<Task>();

        private bool _wait = false;

        public Task(Func<IEnumerator<Task>> func)
        {
            _func = func;
        }

        public void Call(Scheduler scheduler)
        {
            CurrentTask.Value = this;

            if (_enumerator == null)
                _enumerator = _func();
            if(_enumerator.MoveNext())
            {
                if (_enumerator.Current != null)
                {
                    if (!_enumerator.Current._wait)
                    {
                        scheduler.Add(_enumerator.Current);
                    }
                    else
                    {
                        _enumerator.Current._wait = false;
                    }
                }
            }
        }

        public static Task Call(Request request)
        {
            CurrentTask.Value._wait = true;
            request.Target.SendTask(request);
            return CurrentTask.Value;
        }

        public static Task Call(Func<IEnumerator<Task>> func)
        {
            return new Task(func);
        }
    }
}
