﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceProcess;
using System.Text;

namespace ProxyService
{
    static class Program
    {
        /// <summary>
        /// 해당 응용 프로그램의 주 진입점입니다.
        /// </summary>
        static void Main()
        {
            Netronics.Netronics.Service = new Service();
            Netronics.Netronics.SetFlag(Netronics.Netronics.Flag.ServicePort, 10051);
            Netronics.Netronics.Start();

            while (true)
                System.Threading.Thread.Sleep(500);

            return;

            ServiceBase[] ServicesToRun;
            ServicesToRun = new ServiceBase[] 
            { 
                new ProxyService() 
            };
            ServiceBase.Run(ServicesToRun);
        }
    }
}
