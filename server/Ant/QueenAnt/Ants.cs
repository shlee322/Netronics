using System.Collections.Generic;
using Netronics.Channel.Channel;
using Newtonsoft.Json.Linq;

namespace Netronics.Ant.QueenAnt
{
    class Ants
    {
        private int _id;
        private string _name;

        private Ant[] _ant = new Ant[100];
        private int _maxAnt = 0;
        private Queue<int> _removeAnt = new Queue<int>();

        public Ants(int id, string name)
        {
            _id = id;
            _name = name;
        }

        public int GetId()
        {
            return _id;
        }

        public Ant JoinAnt(IChannel channel, JObject args)
        {
            int id = -1;
            if (_removeAnt.Count != 0)
            {
                id = _removeAnt.Dequeue();
            }
            else
            {
                id = _maxAnt++;
            }

            _ant[id] = new Ant(this, id, args.Value<int>("port"));
            _ant[id].AddAddress(args.Value<JArray>("host").Values<byte[]>());
            _ant[id].AddChannel(channel);

            return _ant[id];
        }

        public string GetName()
        {
            return _name;
        }

        public IEnumerable<Ant> GetAnt()
        {
            for (int i = 0; i < _maxAnt; i++)
            {
                if(_ant[i] == null)
                    continue;
                yield return _ant[i];
            }
        }
    }
}
