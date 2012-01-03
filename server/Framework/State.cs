using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;

namespace Netronics
{
    public class State
    {
        static private PerformanceCounter cpu;
        static State()
        {
            cpu = new PerformanceCounter();
            cpu.CategoryName = "Processor";
            cpu.CounterName = "% Processor Time";
            cpu.InstanceName = "_Total";
        }

        public static double getLoad()
        {
            return Math.Pow(1.1, getCPULoad());
        }

        public static float getCPULoad()
        {
            return cpu.NextValue();
        }
    }
}
