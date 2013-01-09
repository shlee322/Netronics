namespace Netronics.Ant.Ant
{
    public class Ants
    {
        private int _id;
        private string _name;

        private IAnt[] _ant = new IAnt[100];
        private int _maxAntId=-1;

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
            if (_maxAntId < ant.GetId())
                _maxAntId = ant.GetId();
        }

        public IAnt GetAnt()
        {
            //원래 랜덤으로 줘야함
            for (int i = 0; i <= _maxAntId; i++)
            {
                if(_ant[i] == null)
                    continue;
                return _ant[i];
            }
            return null;
        }
    }
}
