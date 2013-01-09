using Netronics.Scheduling.Microthreading;
using Newtonsoft.Json.Linq;

namespace Netronics.Ant.Ant
{
    public interface IAnt
    {
        Ants GetAnts();
        int GetId();
        IYield SendTask(int index, JToken message);
        IYield SendMessage(int index, JToken message);
        void SendResponseTask(int tId, JToken message);
    }
}
