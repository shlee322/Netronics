using Netronics.Microthreading;

namespace Netronics.Ant.Ant
{
    public class Ant : IAnt
    {
        public string GetName()
        {
            return null;
        }

        public IYield SendTask(int index)
        {
            throw new System.NotImplementedException();
        }

        public IYield SendMessage(int index)
        {
            throw new System.NotImplementedException();
        }
    }
}
