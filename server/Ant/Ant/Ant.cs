﻿using System.Net;
using Netronics.Scheduling.Microthreading;

namespace Netronics.Ant.Ant
{
    class Ant : IAnt
    {
        private Ants _ants;
        private int _id;

        public Ant(Ants ants, int id)
        {
            _ants = ants;
            _id = id;
        }

        public Ants GetAnts()
        {
            return _ants;
        }

        public int GetId()
        {
            return _id;
        }

        public IYield SendTask(int index)
        {
            throw new System.NotImplementedException();
        }

        public IYield SendMessage(int index)
        {
            throw new System.NotImplementedException();
        }

        public void AddPipeline(IPEndPoint endPoint)
        {
        }
    }
}
