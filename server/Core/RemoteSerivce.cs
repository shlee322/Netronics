using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Netronics
{
    class RemoteSerivce : Serivce
    {
        string serivceName;

        public RemoteSerivce(string serivceName)
        {
            this.serivceName = serivceName;
        }

        public string getSerivceName()
        {
            return this.serivceName;
        }

        public void start()
        {
        }

        public void stop()
        {
        }

        public void processingJob(Job job)
        {
            //패킷전송을 만들자.
        }
    }
}
