﻿
using Netronics.Scheduling.Microthreading;
using Newtonsoft.Json.Linq;

namespace Netronics.Ant.Ant
{
    class LocalAnt : IAnt
    {
        private Ants _ants;
        private AntModel _model;
        private int _id;

        public LocalAnt(Ants ants, AntModel model)
        {
            _ants = ants;
            _model = model;
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
            return null;
        }

        public IYield SendMessage(int index, JToken o)
        {
            return null;
        }

        public void SendResponseTask(int tId, JToken message)
        {
        }
    }
}
