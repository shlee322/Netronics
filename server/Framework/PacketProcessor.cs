﻿using System;
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
        static protected Dictionary<string, LinkedList<Serivce>> globalSerivceList;
        static protected Serivce serivce;
        static protected long transactionID;

        static public void init(Serivce serivce)
        {
            PacketProcessor.transactionID = long.MinValue;
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

        static public void processingPacket(RemoteSerivce serivce, dynamic message)
        {
            string ver = message.v; //버전
            string t = message.t; //트랜젝션 id
            string y = message.y; //타입 q, r

            if (y == "q")
                processingQueryPacket(serivce, message);


        }
        
        static protected String createTransactionID(Job job)
        {
            long id = Interlocked.Increment(ref PacketProcessor.transactionID);
            return "123";
        }

        static public dynamic createQueryPacket(Job job)
        {
            dynamic packet = new JObject();

            packet.v = "1";

            if (job.transaction != null) //트랜젝션 부여가 안됬다.
                packet.t = job.transaction == "" ? createTransactionID(job) : job.transaction;

            packet.y = "q";

            packet.s = job.getSerivceName();
            packet.g = job.group;
            packet.a = job.take;
            packet.m = job.message;

            return packet;
        }

        static protected void processingQueryPacket(RemoteSerivce serivce, dynamic packet)
        {
            Job job = new Job(packet.s);
            job.transaction = packet.t;
            job.group = packet.g;
            job.take = packet.a;
            job.message = packet.m;

            if (job.transaction != null) //트랜젝션 ID가 null일경우 결과 패킷을 전송하지 않아도 됨.
            {
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
            }

            job.setReceiver();

            PacketProcessor.serivce.processingJob(serivce, job);
        }


        static protected void sendJobResult(Job job, bool success)
        {
            dynamic packet = new JObject();
            packet.v = "1";
            packet.t = job.transaction;
            packet.y = "r";
            packet.s = success;
            packet.r = job.result;
            ((RemoteSerivce)job.getSerivce()).sendMessage(packet);
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
