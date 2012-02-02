using System;
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
            HttpProperties properties = new HttpProperties();
            properties.SetHandler(() => new Handler());
            properties.SetIpEndPoint(new IPEndPoint(IPAddress.Any, 8080));

            _netronics = new Netronics.Netronics(properties);
            _netronics.Start();
            Scheduler.SetThreadCount(5);
        }
    }
}
