using System;
using System.Diagnostics;

namespace Netronics
{
    public class State
    {
        private static readonly PerformanceCounter cpu;

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