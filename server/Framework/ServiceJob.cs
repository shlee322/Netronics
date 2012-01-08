using Newtonsoft.Json.Linq;

namespace Netronics
{
    public class ServiceJob : Job
    {
        private ServiceJob()
            : base("all")
        {
        }

        public static ServiceJob Ping(float load)
        {
            var job = new ServiceJob();
            job.Message.s = "Netronics";
            job.Message.t = "ping";
            job.Message.l = load;
            return job;
        }

        public static ServiceJob StartService(Service service)
        {
            var job = new ServiceJob();
            job.ReceiveResult = false;
            job.Message.s = "Netronics";
            job.Message.t = "startService";
            job.Message.n = service.GetServiceName();
            return job;
        }

        public static ServiceJob StopService(Service service)
        {
            var job = new ServiceJob();
            job.ReceiveResult = false;
            job.Message.s = "Netronics";
            job.Message.t = "stopService";
            return job;
        }

        public static ServiceJob ServiceInfo(Service service)
        {
            var job = new ServiceJob();
            job.ReceiveResult = false;
            job.Message.s = "Netronics";
            job.Message.t = "serviceInfo";
            job.Message.n = service.GetServiceName();
            job.Message.g = new JArray(service.GetGroupArray());
            return job;
        }

        public static ServiceJob GetLiveService()
        {
            var job = new ServiceJob();
            job.Message.s = "Netronics";
            job.Message.t = "getLiveService";
            return job;
        }

        public static ServiceJob JoinGroup(string name)
        {
            var job = new ServiceJob();
            job.ReceiveResult = false;
            job.Message.type = "iolnGroup";
            job.Message.name = name;
            return job;
        }

        public static ServiceJob DropGroup(string name)
        {
            var job = new ServiceJob();
            job.ReceiveResult = false;
            job.Message.type = "dropGroup";
            job.Message.name = name;
            return job;
        }
    }
}