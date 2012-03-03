using System;

namespace Framework.Service
{
    class TestProcessor
    {
        public static void aaaa(Netronics.Template.Service.Task.Task task)
        {
            System.Console.WriteLine(DateTime.Now.Ticks);
            TestServer.ExitEvent.Set();
        }
    }
}
