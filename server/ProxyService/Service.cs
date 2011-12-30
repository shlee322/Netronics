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
            e.getJob().result.a = "ok";
            e.getJob().returnResult(this);
        }

        public void start()
        {
            Netronics.Netronics.processingJob(Netronics.ServiceJob.joinGroup("testGroup"));
        }

        public void stop()
        {
        }

        public float getLoad()
        {
            return 0;
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
}
