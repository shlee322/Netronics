using Netronics.Microthreading;

namespace Netronics.Ant.Ant
{
    public interface IAnt
    {
        string GetName();
        IYield SendTask(int index);
        IYield SendMessage(int index);
    }
}
