using System;

namespace Netronics.Template.Service.Task
{
    public class Task
    {
        private Object _msg;
        private Action<Task> _success;
        private Action<Task> _fail;

        public static Task CreateTask(Object msg, Action<Task> success = null, Action<Task> fail = null)
        {
            return new Task(msg, success, fail);
        }

        private Task(Object msg, Action<Task> success, Action<Task> fail)
        {
            _msg = msg;
            _success = success;
            _fail = fail;
        }

        public Object GetMessage()
        {
            return _msg;
        }
    }
}