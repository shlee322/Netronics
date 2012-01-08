using Newtonsoft.Json.Linq;

namespace Netronics
{
    public class ServiceJob : Job
    {
        private ServiceJob()
            : base("all")
        {
        }

        public static ServiceJob ping(float load)
        {
            var job = new ServiceJob();
            job.message.s = "Netronics";
            job.message.t = "ping";
            job.message.l = load;
            return job;
        }

        public static ServiceJob startService(Service Service)
        {
            var job = new ServiceJob();
            job.receiveResult = false;
            job.message.s = "Netronics";
            job.message.t = "startService";
            job.message.n = Service.getServiceName();
            return job;
        }

        public static ServiceJob stopService(Service Service)
        {
            var job = new ServiceJob();
            job.receiveResult = false;
            job.message.s = "Netronics";
            job.message.t = "stopService";
            return job;
        }

        public static ServiceJob serviceInfo(Service Service)
        {
            var job = new ServiceJob();
            job.receiveResult = false;
            job.message.s = "Netronics";
            job.message.t = "serviceInfo";
            job.message.n = Service.getServiceName();
            job.message.g = new JArray(Service.getGroupArray());
            return job;
        }

        public static ServiceJob getLiveService()
        {
            var job = new ServiceJob();
            job.message.s = "Netronics";
            job.message.t = "getLiveService";
            return job;
        }

        public static ServiceJob joinGroup(string name)
        {
            var job = new ServiceJob();
            job.receiveResult = false;
            job.message.type = "iolnGroup";
            job.message.name = name;
            return job;
        }

        public static ServiceJob dropGroup(string name)
        {
            var job = new ServiceJob();
            job.receiveResult = false;
            job.message.type = "dropGroup";
            job.message.name = name;
            return job;
        }
    }
}