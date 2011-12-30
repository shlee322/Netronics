using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json.Linq;

namespace Netronics
{
    public class SerivceJob : Job
    {
        static public SerivceJob ping(float load)
        {
            SerivceJob job = new SerivceJob();
            job.message.s = "Netronics";
            job.message.t = "ping";
            job.message.l = load;
            return job;
        }

        static public SerivceJob startSerivce(Serivce serivce)
        {
            SerivceJob job = new SerivceJob();
            job.message.s = "Netronics";
            job.message.t = "startService";
            job.message.n = serivce.getSerivceName();
            return job;
        }

        static public SerivceJob stopService(Serivce serivce)
        {
            SerivceJob job = new SerivceJob();
            job.message.s = "Netronics";
            job.message.t = "stopService";
            return job;
        }

        static public SerivceJob serviceInfo(Serivce serivce)
        {
            SerivceJob job = new SerivceJob();
            job.receiveResult = false;
            job.message.s = "Netronics";
            job.message.t = "serviceInfo";
            job.message.n = serivce.getSerivceName();
            job.message.g = new JArray(serivce.getGroupArray());
            return job;
        }

        static public SerivceJob getLiveSerivce()
        {
            SerivceJob job = new SerivceJob();
            job.message.s = "Netronics";
            job.message.t = "getLiveSerivce";
            return job;
        }

        static public SerivceJob liveServiceList(List<object> serivceInfoList)
        {
            SerivceJob job = new SerivceJob();
            job.message.s = "Netronics";
            job.message.t = "liveServiceList";
            job.message.e = serivceInfoList;
            return job;
        }

        static public SerivceJob joinGroup(string name)
        {
            SerivceJob job = new SerivceJob();
            job.message.type = "iolnGroup";
            job.message.name = name;
            return job;
        }

        static public SerivceJob dropGroup(string name)
        {
            SerivceJob job = new SerivceJob();
            job.message.type = "dropGroup";
            job.message.name = name;
            return job;
        }

        private SerivceJob()
            : base("all")
        {
        }
    }
}
