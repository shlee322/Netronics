using System.Net;
using Netronics.Ant.Ant.Network;
using Netronics.Channel.Channel;
using Netronics.Scheduling.Microthreading;
using Newtonsoft.Json.Linq;

namespace Netronics.Ant.Ant
{
    class RemoteAnt : IAnt
    {
        private Ants _ants;
        private int _id;
        private Transaction.RemoteTransaction _transaction;

        public RemoteAnt(Ants ants, int id)
        {
            _ants = ants;
            _id = id;
            _transaction = new Transaction.RemoteTransaction(this);
        }

        public Ants GetAnts()
        {
            return _ants;
        }

        public int GetId()
        {
            return _id;
        }

        public IYield SendTask(int index, JToken o)
        {
            return _transaction.SendTask(index, o);
        }

        public IYield SendMessage(int index, JToken o)
        {
            return Microthread.None();
        }

        public void SendResponseTask(int tId, JToken message)
        {
            _transaction.SendResponseTask(tId, message);
        }

        public void AddChannel(IChannel channel)
        {
            channel.SetTag(this);

            _transaction.AddChannel(channel);
        }

        public void ReceiveTask(int tId, int type, JToken o)
        {
            var taskObj = new Task(this, tId, type, o);
            var task = Kernel.GetKernel().GetTask(type);
            Scheduling.Scheduler.Default.RunMicrothread(tId, new Microthread(() => task(taskObj)));
        }

        public void ResponseTask(int tId, JToken o)
        {
            _transaction.ResponseTask(tId, o);
        }
    }
}
