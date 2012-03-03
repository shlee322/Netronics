using System;
using Netronics.Channel.Channel;
using Netronics.Template.Service.Protocol;

namespace Netronics.Template.Service.Task
{
    /// <summary>
    /// 서비스끼리의 통신은 서비스ID부여를 제외한 모두 Task를 통해서 진행된다.
    /// </summary>
    public class Task
    {
        private IChannel _sender;
        private Request _request;
        private readonly Object _msg;
        private Action<Task, object> _success;
        private Action<Task, object> _fail;

        public static Task CreateTask(Object msg, Action<Task, object> success = null, Action<Task, object> fail = null)
        {
            return new Task(msg, success, fail);
        }

        public static Task GetTask(IChannel channel, Request request)
        {
            var task = new Task(request.Message, null, null) { _sender = channel, _request = request };
            return task;
        }

        private Task(Object msg, Action<Task, object> success, Action<Task, object> fail)
        {
            _msg = msg;
            _success = success;
            _fail = fail;
        }

        public Object GetMessage()
        {
            return _msg;
        }

        public bool IsReceiveResult()
        {
            return _success != null || _fail != null;
        }

        public void Result(object resultObject, bool success=true)
        {
            if (_request == null)
            {
                if (success && _success != null)
                    _success(this, resultObject);
                else if (!success && _fail != null)
                    _fail(this, resultObject);
                return;
            }
            if (!_request.Result)
                throw new Exception("결과값을 필요로 하지 않는 Task입니다");
            var result = new Result { Success = success, Sender = _request.Receiver, Receiver = _request.Sender, Transaction = _request.Transaction, ResultObject = resultObject };
            _sender.SendMessage(result);
        }
    }
}