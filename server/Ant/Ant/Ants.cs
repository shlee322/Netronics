using System.Collections.Concurrent;

namespace Netronics.Ant.Ant
{
    public class Ants
    {
        private int _id;
        private string _name;

        private IAnt[] _ant = new IAnt[100];
        private int _maxAntId=-1;
        private ConcurrentQueue<int> _connectAnt = new ConcurrentQueue<int>(); 

        public static Ants GetAnts(string name)
        {
            return Kernel.GetKernel().GetAnts(name);
        }

        public Ants(int id, string name)
        {
            _id = id;
            _name = name;
        }

        public int GetId()
        {
            return _id;
        }

        public void JoinAnt(IAnt ant)
        {
            _ant[ant.GetId()] = ant;
            _connectAnt.Enqueue(ant.GetId());
            if (_maxAntId < ant.GetId())
                _maxAntId = ant.GetId();
        }

        public IAnt GetAnt()
        {
            int id;
            if (!_connectAnt.TryPeek(out id))
                return null;
            var ant = _ant[id];
            _connectAnt.Enqueue(id);
            return ant;
        }

        public IAnt GetAnt(int id)
        {
            return _ant[id];
        }
    }
}
