using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json.Linq;
using System.Threading.Tasks;

namespace Netronics
{
    public class PacketProcessor
    {
        static protected Dictionary<string, LinkedList<Serivce>> globalSerivceList;
        static protected Serivce serivce;

        static public void init(Serivce serivce)
        {
            PacketProcessor.serivce = serivce;
            PacketProcessor.initSerivceList();
        }

        static protected void initSerivceList()
        {
            PacketProcessor.globalSerivceList = new Dictionary<string, LinkedList<Serivce>>();
            LinkedList<Serivce> mySerivceType = new System.Collections.Generic.LinkedList<Serivce>();
            mySerivceType.AddFirst(PacketProcessor.serivce);
            PacketProcessor.globalSerivceList.Add(PacketProcessor.serivce.getSerivceName(), mySerivceType);
        }

        static public void processingPacket(RemoteSerivce serivce, dynamic packet)
        {
            if (((string)packet.type) != "Netronics")
            {
                PacketProcessor.processingJobPacket(serivce, packet);
                return;
            }

            packet = packet.netronics;

            switch ((string)packet.type)
            {
                case "ping":
                    dynamic data = new JObject();
                    data.type = "pong";
                    //여기서 패킷전송
                    break;
                case "startService":
                    break;
                case "getLiveService":
                    break;
            }
        }

        static protected void processingJobPacket(RemoteSerivce serivce, dynamic packet)
        {
            Job job = new Job(packet.serivce);
            job.group = packet.netronics.group;
            job.take = packet.netronics.take;
            job.message = packet.netronics.message;
            job.success += new Job.Result(
                delegate(Job j)
                {
                    PacketProcessor.sendJobResult(j, true);
                }
                );
            job.fail += new Job.Result(
                delegate(Job j)
                {
                    PacketProcessor.sendJobResult(j, false);
                }
                );
            job.setReceiver();

            PacketProcessor.serivce.processingJob(serivce, job);
        }

        static protected dynamic createJobResultMessage(string transactionID, bool success, dynamic result)
        {
            dynamic packet = new JObject();
            packet.type = "Netronics";
            packet.netronics = new JObject();
            packet.netronics.type = "returnJobResult";
            packet.netronics.transaction = transactionID;
            packet.netronics.success = success;
            packet.netronics.result = result;

            return packet;
        }

        static protected void sendJobResult(Job job, bool success)
        {
            ((RemoteSerivce)job.getSerivce()).sendMessage(createJobResultMessage(job.getTransactionID(), success, job.result));
        }

        static public void processingJob(Job job)
        {
            string processingGroup = job.group;

            //이럼 속도의 문제가 좀 있을것 같음.
            //나중에 캐싱을 하던지 해보자.
            IEnumerable<Serivce> serivceList =
                from serivce in PacketProcessor.globalSerivceList[job.getSerivceName()].AsParallel()
                where serivce.isGroup(processingGroup)
                orderby serivce.getLoad() ascending
                select serivce;

            if (job.take > 0)
                serivceList = serivceList.Take(job.take);

            Parallel.ForEach(serivceList, serivce =>
            {
                serivce.processingJob(PacketProcessor.serivce, job);
            });
        }
    }
}
