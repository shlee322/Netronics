using System.Collections.Concurrent;
using Netronics.Channel.Channel;
using Netronics.Scheduling.Microthreading;
using Newtonsoft.Json.Linq;

namespace Netronics.Ant.Ant.Transaction
{
    class Transaction
    {
        private RemoteAnt _ant;
        private IChannel _channel;
        private Task[] _tasks = new Task[10000]; //만약 지정 갯수보다 많으면 큐에 넣고 대기
        private ConcurrentQueue<int> _taskIndex = new ConcurrentQueue<int>(); 

        public Transaction(RemoteAnt ant)
        {
            _ant = ant;
            for (int i = 0; i < _tasks.Length; i++)
                _taskIndex.Enqueue(i);
        }

        public void AddChannel(IChannel channel)
        {
            _channel = channel;
        }

        public IYield SendTask(int index, JToken o)
        {
            int id = GenerateTransactionId();
            if (id == -1)
                return Microthread.Wait(new WaitEvent()); //구현중
            _tasks[id] = new Task();
            var packet = new JObject();
            packet.Add("type", "request");
            packet.Add("t_id", id);
            packet.Add("m_type", index);
            packet.Add("args", o);
            _channel.SendMessage(packet);

            return Microthread.Wait(_tasks[id].GetWaitEvent());
        }

        private int GenerateTransactionId()
        {
            int id = -1;
            if (_taskIndex.TryDequeue(out id))
                return id;
            return -1;
        }

        public void SendResponseTask(int tId, JToken message)
        {
            var packet = new JObject();
            packet.Add("type", "response");
            packet.Add("t_id", tId);
            packet.Add("args", message);
            _channel.SendMessage(packet);
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
