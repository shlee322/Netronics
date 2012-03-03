using System;
using Netronics.Template.Service.Protocol;

namespace Netronics.Template.Service.Task
{
    /// <summary>
    /// 서비스끼리의 통신은 서비스ID부여를 제외한 모두 Task를 통해서 진행된다.
    /// </summary>
    public class Task
    {
        private Object _msg;
        private Action<Task> _success;
        private Action<Task> _fail;

        public static Task CreateTask(Object msg, Action<Task> success = null, Action<Task> fail = null)
        {
            return new Task(msg, success, fail);
        }

        public static Task GetTask(Request request)
        {
            var task = new Task(request.Message, task1 => { }, task1 => { }); //성공 실패시 여기서 보내야할까?
            return task;
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