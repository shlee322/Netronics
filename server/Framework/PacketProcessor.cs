using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json.Linq;
using System.Threading.Tasks;
using System.Threading;

namespace Netronics
{
    public class PacketProcessor
    {
        static protected Dictionary<string, LinkedList<Service>> globalServiceList;
        static protected Service service;

        static public void init(Service service)
        {
            PacketProcessor.service = service;
            PacketProcessor.initServiceList();
        }

        static protected void initServiceList()
        {
            PacketProcessor.globalServiceList = new Dictionary<string, LinkedList<Service>>();
            LinkedList<Service> myServiceType = new System.Collections.Generic.LinkedList<Service>();
            myServiceType.AddFirst(PacketProcessor.service);
            PacketProcessor.globalServiceList.Add(PacketProcessor.service.getServiceName(), myServiceType);
        }

        static public bool processingPacket(RemoteService service, dynamic message)
        {
            string ver = message.v; //버전
            string t = message.t; //트랜젝션 id
            string y = message.y; //타입 q, r

            if (y == "q")
            {
                processingQueryPacket(service, message);
                return true;
            }

            return false;
        }

        static public dynamic createQueryPacket(string transactionID, Job job)
        {
            dynamic packet = new JObject();

            packet.v = "1";

            if(transactionID != null)
                packet.t = transactionID;

            packet.y = "q";

            packet.s = job.getServiceName();
            packet.g = job.group;
            packet.a = job.take;
            packet.m = job.message;

            return packet;
        }

        static protected void processingQueryPacket(RemoteService service, dynamic packet)
        {
            Job job = new Job(packet.s);
            job.group = packet.g;
            job.take = packet.a;
            job.message = packet.m;

            if (packet.t != null) //트랜젝션 ID가 null일경우 결과 패킷을 전송하지 않아도 됨.
            {
                job.success += new Job.Result(
                        delegate(Service sender, Job.ResultEventArgs e)
                        {
                            PacketProcessor.sendJobResult(e.getJob(), true);
                        }
                    );
                job.fail += new Job.Result(
                        delegate(Service sender, Job.ResultEventArgs e)
                        {
                            PacketProcessor.sendJobResult(e.getJob(), false);
                        }
                    );
            }
            else
            {
                job.receiveResult = false;
            }

            job.setReceiver();
            job.transaction = packet.t;

            PacketProcessor.service.processingJob(service, job);
        }


        static protected void sendJobResult(Job job, bool success)
        {
            dynamic packet = new JObject();
            packet.v = "1";
            if(packet.t != null)
                packet.t = job.transaction;
            packet.y = "r";
            packet.s = success;
            packet.r = job.result;
            ((RemoteService)job.getService()).sendMessage(packet);
        }

        static public void processingJob(Job job)
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
