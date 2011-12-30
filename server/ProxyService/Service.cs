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
            divider.addProcessor(
                delegate (AutoTypeDivider.DividerEventArgs e)
                {
                    return true;
                }, null);
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
