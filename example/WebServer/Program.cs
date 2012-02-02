using System.Net;
using Netronics;
using Netronics.Template.HTTP;

namespace WebServer
{
    class Program
    {
        private static Netronics.Netronics _netronics;
        static void Main(string[] args)
        {
            _netronics = new Netronics.Netronics(
                ((HttpProperties)new HttpProperties()
                .SetIpEndPoint(new IPEndPoint(IPAddress.Any, 8080)))
                .SetHandler(() => new Handler()))
                .Start();
            Scheduler.SetThreadCount(5);
        }
    }
}
