using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json.Linq;

namespace Netronics
{
    public class ServiceJob : Job
    {
        static public ServiceJob ping(float load)
        {
            ServiceJob job = new ServiceJob();
            job.message.s = "Netronics";
            job.message.t = "ping";
            job.message.l = load;
            return job;
        }

        static public ServiceJob startService(Service Service)
        {
            ServiceJob job = new ServiceJob();
			job.receiveResult = false;
            job.message.s = "Netronics";
            job.message.t = "startService";
            job.message.n = Service.getServiceName();
            return job;
        }

        static public ServiceJob stopService(Service Service)
        {
            ServiceJob job = new ServiceJob();
			job.receiveResult = false;
            job.message.s = "Netronics";
            job.message.t = "stopService";
            return job;
        }

        static public ServiceJob serviceInfo(Service Service)
        {
            ServiceJob job = new ServiceJob();
            job.receiveResult = false;
            job.message.s = "Netronics";
            job.message.t = "serviceInfo";
            job.message.n = Service.getServiceName();
            job.message.g = new JArray(Service.getGroupArray());
            return job;
        }

        static public ServiceJob getLiveService()
        {
            ServiceJob job = new ServiceJob();
            job.message.s = "Netronics";
            job.message.t = "getLiveService";
            return job;
        }

        static public ServiceJob joinGroup(string name)
        {
            ServiceJob job = new ServiceJob();
			job.receiveResult = false;
            job.message.type = "iolnGroup";
            job.message.name = name;
            return job;
        }

        static public ServiceJob dropGroup(string name)
        {
            ServiceJob job = new ServiceJob();
			job.receiveResult = false;
            job.message.type = "dropGroup";
            job.message.name = name;
            return job;
        }

        private ServiceJob()
            : base("all")
        {
        }
    }
}
