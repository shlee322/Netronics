using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Netronics;

namespace ProxyService
{
    class Service : Serivce
    {
        AutoTypeDivider divider = new AutoTypeDivider();

        public Service()
        {
        }

        public string getSerivceName()
        {
            return "Proxy";
        }

        public void init()
        {
            this.divider.addProcessor(
                delegate (AutoTypeDivider.DividerEventArgs e)
                {
                    return e.getJob().message.type == "test";
                }, this.test);
        }

        private void test(AutoTypeDivider.DividerEventArgs e)
        {
            e.getJob().result.a = "ok";
            e.getJob().returnResult(this);
        }

        public void start()
        {
            Netronics.Netronics.processingJob(Netronics.SerivceJob.joinGroup("testGroup"));
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

        public void processingJob(Netronics.Serivce serivce, Netronics.Job job)
        {
            divider.processingJob(serivce, job);
        }
    }
}
