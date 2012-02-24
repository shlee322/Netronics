using System;

namespace Netronics.Template.Service.Task
{
    internal class Task
    {
        private Object _msg;
        private Action<Task> _success;
        private Action<Task> _fail;

        public static Task CreateTask(Object msg, Action<Task> success, Action<Task> fail)
        {
            return new Task(msg, success, fail);
        }

        private Task(Object msg, Action<Task> success, Action<Task> fail)
        {
        }
    }
}