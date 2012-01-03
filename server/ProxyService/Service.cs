using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ProxyService
{
    class Service : Netronics.Service
    {
        Netronics.AutoTypeDivider divider = new Netronics.AutoTypeDivider();

        public Service()
        {
        }

        public string getServiceName()
        {
            return "Proxy";
        }

        public void init()
        {
            this.divider.addProcessor(
                delegate(Netronics.AutoTypeDivider.DividerEventArgs e)
                {
                    return e.getJob().message.type == "test";
                }, this.test);
        }

        private void test(Netronics.AutoTypeDivider.DividerEventArgs e)
        {
            e.getJob().result.time = e.getJob().message.time;
            e.getJob().returnResult(this);
            //e.getJob().getService().processingJob(this, new TestJob());
        }

        public void start()
        {
            Netronics.Netronics.processingJob(Netronics.ServiceJob.joinGroup("testGroup"));
        }

        public void stop()
        {
        }

        public double getLoad()
        {
            return Netronics.State.getLoad();
        }

        public string[] getGroupArray()
        {
            return new string[] { };
        }

        public bool isGroup(string group)
        {
            return false;
        }

        public void processingJob(Netronics.Service Service, Netronics.Job job)
        {
            divider.processingJob(Service, job);
        }
    }

    class TestJob : Netronics.Job
    {
        public TestJob()
            : base("Test")
        {
            this.message.test = "test";
            this.success += new Result(TestJob_success);
        }

        void TestJob_success(Netronics.Service sender, Netronics.Job.ResultEventArgs e)
        {
        }
    }
}
