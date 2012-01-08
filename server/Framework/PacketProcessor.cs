using System.Collections.Generic;
using Newtonsoft.Json.Linq;

namespace Netronics
{
    public class PacketProcessor
    {
        protected static Dictionary<string, LinkedList<Service>> globalServiceList;
        protected static Service service;

        public static void init(Service service)
        {
            PacketProcessor.service = service;
            initServiceList();
        }

        protected static void initServiceList()
        {
            globalServiceList = new Dictionary<string, LinkedList<Service>>();
            var myServiceType = new LinkedList<Service>();
            myServiceType.AddFirst(service);
            globalServiceList.Add(service.getServiceName(), myServiceType);
        }

        public static string getPacketType(RemoteService service, dynamic message)
        {
            return message.y;
        }

        public static void processingPacket(RemoteService service, dynamic message)
        {
            /*
            string ver = message.v; //버전
            string t = message.t; //트랜젝션 id
            string y = message.y; //타입 q, r
            */
            processingQueryPacket(service, message);
        }

        public static dynamic createQueryPacket(string transactionID, Job job)
        {
            dynamic packet = new JObject();

            packet.v = "1";

            if (transactionID != null)
                packet.t = transactionID;

            packet.y = "q";

            packet.m = job.message;

            return packet;
        }

        protected static void processingQueryPacket(RemoteService service, dynamic packet)
        {
            var job = new Job(service);
            job.message = packet.m;

            if (packet.t != null) //트랜젝션 ID가 null일경우 결과 패킷을 전송하지 않아도 됨.
            {
                job.success += delegate(Service sender, Job.ResultEventArgs e)
                                   {
                                       sendJobResult(e.getJob(), true);
                                       job.Dispose();
                                   };
                job.fail += delegate(Service sender, Job.ResultEventArgs e)
                                {
                                    sendJobResult(e.getJob(), false);
                                    job.Dispose();
                                };
            }
            else
            {
                job.receiveResult = false;
            }

            job.setReceiver();
            job.transaction = packet.t;

            PacketProcessor.service.processingJob(service, job);

            if (!job.receiveResult)
                job.Dispose();
        }


        protected static void sendJobResult(Job job, bool success)
        {
            dynamic packet = new JObject();
            packet.v = "1";
            if (packet.t != null)
                packet.t = job.transaction;
            packet.y = "r";
            if (success)
                packet.r = job.result;
            else
                packet.f = true;

            if (!((RemoteService) job.getService()).sendMessage(packet))
                job.returnResult(job.getService(), false);
        }

        public static void processingJob(Job job)
        {
            string processingGroup = job.group;

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