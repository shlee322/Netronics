
using System.Collections.Concurrent;
using Netronics.Scheduling.Microthreading;
using Newtonsoft.Json.Linq;

namespace Netronics.Ant.Ant
{
    class LocalAnt : IAnt
    {
        private Ants _ants;
        private AntModel _model;
        private int _id;

        private Transaction.LocalTransaction _transaction;

        public LocalAnt(Ants ants, AntModel model)
        {
            _ants = ants;
            _model = model;
            _transaction = new Transaction.LocalTransaction(this);
        }

        public AntModel GetModel()
        {
            return _model;
        }

        public Ants GetAnts()
        {
            return _ants;
        }

        public void SetId(int id)
        {
            _id = id;
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
            _transaction.ResponseTask(tId, message);
        }
    }
}
