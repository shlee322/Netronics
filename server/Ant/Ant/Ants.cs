namespace Netronics.Ant.Ant
{
    public class Ants
    {
        private int _id;
        private string _name;

        private IAnt[] _ant = new IAnt[100];

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
        }
    }
}
