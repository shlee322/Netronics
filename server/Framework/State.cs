using System;
using System.Diagnostics;

namespace Netronics
{
    public class State
    {
        private static readonly PerformanceCounter CPU;

        static State()
        {
            CPU = new PerformanceCounter();
            CPU.CategoryName = "Processor";
            CPU.CounterName = "% Processor Time";
            CPU.InstanceName = "_Total";
        }

        public static double GetLoad()
        {
            return Math.Pow(1.1, GetCPULoad());
        }

        public static float GetCPULoad()
        {
            return CPU.NextValue();
        }
    }
}