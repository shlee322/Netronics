using System.Collections.Concurrent;
using System.Threading;
using Netronics.Channel;
using Netronics.Channel.Channel;
using Netronics.Template.Service.Protocol;

namespace Netronics.Template.Service.Service
{
    internal sealed class RemoteService : Service
    {
        private readonly Task.Task[] _transactionList = new Task.Task[ServiceManager.TransactionSize];
        private readonly ConcurrentQueue<int> _archivedTransactionID = new ConcurrentQueue<int>();
        private int _maxTransactionID = 0;
        private IChannel _channel;
        private readonly int _id;

        public RemoteService(ServiceManager manager, int id)
        {
            SetServiceManager(manager);
            _id = id;
        }

        public void SetChannel(IChannel channel)
        {
            _channel = channel;
        }

        public override int GetID()
        {
            return _id;
        }

        public override void ProcessingTask(Task.Task task)
        {
            int id = GetTransactionID();
            _transactionList[id] = task;

            var request = new Request()
                              {
                                  Result = task.IsReceiveResult(),
                                  Sender = GetServiceManager().GetLocalService().GetID(),
                                  Receiver = GetID(),
                                  Transaction = (uint) id,
                                  Message = task.GetMessage()
                              };
            _channel.SendMessage(request);
        }

        private int GetTransactionID()
        {
            int id = 0;
            if(!_archivedTransactionID.TryDequeue(out id))
            {
                id = Interlocked.Increment(ref _maxTransactionID);
                return id - 1;
            }
            return id;
        }

        public Task.Task GetOrRemoveTransaction(int id)
        {
            Task.Task task = _transactionList[id];
            _transactionList[id] = null;
            if(task != null)
                _archivedTransactionID.Enqueue(id);
            return task;
        }
    }
}