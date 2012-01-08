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
            job.Message.s = "Netronics";
            job.Message.t = "ping";
            job.Message.l = load;
            return job;
        }

        public static ServiceJob startService(Service Service)
        {
            var job = new ServiceJob();
            job.ReceiveResult = false;
            job.Message.s = "Netronics";
            job.Message.t = "startService";
            job.Message.n = Service.GetServiceName();
            return job;
        }

        public static ServiceJob stopService(Service Service)
        {
            var job = new ServiceJob();
            job.ReceiveResult = false;
            job.Message.s = "Netronics";
            job.Message.t = "stopService";
            return job;
        }

        public static ServiceJob serviceInfo(Service Service)
        {
            var job = new ServiceJob();
            job.ReceiveResult = false;
            job.Message.s = "Netronics";
            job.Message.t = "serviceInfo";
            job.Message.n = Service.GetServiceName();
            job.Message.g = new JArray(Service.GetGroupArray());
            return job;
        }

        public static ServiceJob getLiveService()
        {
            var job = new ServiceJob();
            job.Message.s = "Netronics";
            job.Message.t = "getLiveService";
            return job;
        }

        public static ServiceJob joinGroup(string name)
        {
            var job = new ServiceJob();
            job.ReceiveResult = false;
            job.Message.type = "iolnGroup";
            job.Message.name = name;
            return job;
        }

        public static ServiceJob dropGroup(string name)
        {
            var job = new ServiceJob();
            job.ReceiveResult = false;
            job.Message.type = "dropGroup";
            job.Message.name = name;
            return job;
        }
    }
}