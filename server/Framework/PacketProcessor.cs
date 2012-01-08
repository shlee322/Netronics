using System.Collections.Generic;
using Newtonsoft.Json.Linq;

namespace Netronics
{
    public class PacketProcessor
    {
        protected static Dictionary<string, LinkedList<Service>> GlobalServiceList;
        protected static Service _Service;

        public static void Init(Service service)
        {
            PacketProcessor._Service = service;
            InitServiceList();
        }

        protected static void InitServiceList()
        {
            GlobalServiceList = new Dictionary<string, LinkedList<Service>>();
            var myServiceType = new LinkedList<Service>();
            myServiceType.AddFirst(_Service);
            GlobalServiceList.Add(_Service.GetServiceName(), myServiceType);
        }

        public static string GetPacketType(RemoteService service, dynamic message)
        {
            return message.y;
        }

        public static void ProcessingPacket(RemoteService service, dynamic message)
        {
            /*
            string ver = message.v; //버전
            string t = message.t; //트랜젝션 id
            string y = message.y; //타입 q, r
            */
            ProcessingQueryPacket(service, message);
        }

        public static dynamic CreateQueryPacket(string transactionID, Job job)
        {
            dynamic packet = new JObject();

            packet.v = "1";

            if (transactionID != null)
                packet.t = transactionID;

            packet.y = "q";

            packet.m = job.Message;

            return packet;
        }

        protected static void ProcessingQueryPacket(RemoteService service, dynamic packet)
        {
            var job = new Job(service);
            job.Message = packet.m;

            if (packet.t != null) //트랜젝션 ID가 null일경우 결과 패킷을 전송하지 않아도 됨.
            {
                job.Success += delegate(Service sender, Job.ResultEventArgs e)
                                   {
                                       SendJobResult(e.GetJob(), true);
                                       job.Dispose();
                                   };
                job.Fail += delegate(Service sender, Job.ResultEventArgs e)
                                {
                                    SendJobResult(e.GetJob(), false);
                                    job.Dispose();
                                };
            }
            else
            {
                job.ReceiveResult = false;
            }

            job.SetReceiver();
            job.Transaction = packet.t;

            PacketProcessor._Service.ProcessingJob(service, job);

            if (!job.ReceiveResult)
                job.Dispose();
        }


        protected static void SendJobResult(Job job, bool success)
        {
            dynamic packet = new JObject();
            packet.v = "1";
            if (packet.t != null)
                packet.t = job.Transaction;
            packet.y = "r";
            if (success)
                packet.r = job.Result;
            else
                packet.f = true;

            if (!((RemoteService) job.GetService()).SendMessage(packet))
                job.ReturnResult(job.GetService(), false);
        }

        public static void ProcessingJob(Job job)
        {
            string processingGroup = job.Group;

            //이럼 속도의 문제가 좀 있을것 같음.
            //나중에 캐싱을 하던지 해보자.
            /*
            IEnumerable<Service> ServiceList =
                from Service in PacketProcessor.globalServiceList[job.getServiceName()].AsParallel()
                where Service.isGroup(processingGroup)
                orderby Service.getLoad() ascending
                select Service;

            if (job.take > 0)
                ServiceList = ServiceList.Take(job.take);

            Parallel.ForEach(ServiceList, Service =>
            {
                Service.processingJob(PacketProcessor.Service, job);
            });*/
        }
    }
}