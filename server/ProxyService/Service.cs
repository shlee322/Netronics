using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ProxyService
{
    class Service : Netronics.Service
    {
        private Netronics.AutoTypeDivider divider = new Netronics.AutoTypeDivider();
		private bool run = false;

        public Service()
        {
        }

        public string GetServiceName()
        {
            return "Proxy";
        }

        public void Init()
        {
            this.divider.AddProcessor(
                delegate(Netronics.AutoTypeDivider.DividerEventArgs e)
                {
                    return e.GetJob().Message.type == "test";
                }, this.test);
        }

        private void test(Netronics.AutoTypeDivider.DividerEventArgs e)
        {
            e.GetJob().Result.time = e.GetJob().Message.time;
            e.GetJob().ReturnResult(this);
            //e.getJob().getService().processingJob(this, new TestJob());
        }

        public void Start()
        {
            this.run = true;
        }

        public void Stop()
        {
			this.run = false;
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
            divider.ProcessingJob(service, job);
        }

        public bool GetRunning()
        {
            return this.run;
        }
    }

    class TestJob : Netronics.Job
    {
        public TestJob()
            : base("Test")
        {
            this.Message.test = "test";
            this.Success += new ResultDelegate(TestJob_success);
        }

        void TestJob_success(Netronics.Service sender, Netronics.Job.ResultEventArgs e)
        {
        }
    }
}
