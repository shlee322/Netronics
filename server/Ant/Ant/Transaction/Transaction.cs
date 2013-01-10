using System.Collections.Concurrent;
using Netronics.Channel.Channel;
using Netronics.Scheduling.Microthreading;
using Newtonsoft.Json.Linq;

namespace Netronics.Ant.Ant.Transaction
{
    class Transaction
    {
        
        protected Task[] _tasks = new Task[10000]; //만약 지정 갯수보다 많으면 큐에 넣고 대기
        protected ConcurrentQueue<int> _taskIndex = new ConcurrentQueue<int>(); 

        public Transaction()
        {
            for (int i = 0; i < _tasks.Length; i++)
                _taskIndex.Enqueue(i);
        }

        
        public IYield SendTask(int index, JToken o)
        {
            int id = GenerateTransactionId();
            if (id == -1)
                return Microthread.Wait(new WaitEvent()); //구현중
            _tasks[id] = new Task();
            SendTask2(id, index, o);

            //로컬에서 너무 처리가 빨라 이함수가 리턴되기 전에 ResponseTask가 호출되어 task를 삭제하는것을 방지
            var task = _tasks[id];
            if (task == null)
                return Microthread.None();
            return Microthread.Wait(task.GetWaitEvent());
        }

        protected virtual void SendTask2(int tid, int index, JToken o)
        {
        }

        private int GenerateTransactionId()
        {
            int id = -1;
            if (_taskIndex.TryDequeue(out id))
                return id;
            return -1;
        }

        public void ResponseTask(int tId, JToken o)
        {
            var task = _tasks[tId];
            _tasks[tId] = null;
            _taskIndex.Enqueue(tId);
            task.Run(o);
        }
    }
}
