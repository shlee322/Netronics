using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ProxyService
{
    class Service : Netronics.Service
    {
		private bool _run = false;

        public string GetServiceName()
        {
            return "Proxy";
        }

        public void Init()
        {
        }

        public void Start()
        {
            this._run = true;
        }

        public void Stop()
        {
			this._run = false;
        }

        public double GetLoad()
        {
            return Netronics.State.GetLoad();
        }

        public string[] GetGroupArray()
        {
            return new string[] { };
        }

        public bool IsGroup(string group)
        {
            return false;
        }

        public void ProcessingJob(Netronics.Service service, Netronics.Job job)
        {
			job.AddProcessor();
        }

        public bool GetRunning()
        {
            return this._run;
        }
    }
}
