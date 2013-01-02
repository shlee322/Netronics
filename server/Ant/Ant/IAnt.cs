using Netronics.Scheduling.Microthreading;

namespace Netronics.Ant.Ant
{
    public interface IAnt
    {
        Ants GetAnts();
        int GetId();
        IYield SendTask(int index);
        IYield SendMessage(int index);
    }
}
