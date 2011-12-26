using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ProxyService
{
    class Service : Netronics.Serivce
    {
        public Service()
        {
        }

        public string getSerivceName()
        {
            return "Proxy";
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

        public bool isGroup(string group)
        {
            return false;
        }

        public void processingJob(Netronics.Serivce serivce, Netronics.Job job)
        {
        }
    }
}
