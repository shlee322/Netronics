using System.Collections.Generic;
using Netronics.Ant.Ant;
using Netronics.Scheduling.Microthreading;
using Newtonsoft.Json.Linq;

namespace Test1Ant
{
    public class Test1 : AntModel
    {
        public override string GetName()
        {
            return "Test1";
        }

        public override void OnStart()
        {
            AddTask(0, Task1);

            Netronics.Scheduling.Scheduler.Default.RunMicrothread(0, new Microthread(()=>TestMethod1()));
        }

        private IEnumerator<IYield> Task1(Task task)
        {
            System.Console.WriteLine("test");
            task.Response(new JValue(321));

            Netronics.Scheduling.Scheduler.Default.RunMicrothread(0, new Microthread(()=>TestMethod1(task.GetAnt())));
            yield return null;
        }

        private IEnumerator<IYield> TestMethod1(IAnt ant = null)
        {
            yield return Microthread.Sleep(5);
            if(ant == null)
                ant = Ants.GetAnts("Test1").GetAnt();
            yield return ant.SendTask(0, new JValue(123));
            System.Console.WriteLine("Result : " + Microthread.CurrentMicrothread.Result);
        }
    }
}
