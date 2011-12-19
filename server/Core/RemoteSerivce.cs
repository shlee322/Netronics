using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Sockets;

namespace Netronics
{
    class RemoteSerivce : Serivce
    {
        protected Socket oSocket;
        protected string serivceName;

        public RemoteSerivce(Socket socket)
        {
            this.oSocket = socket;
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
