﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Sockets;
using System.Net;
using Newtonsoft.Json.Linq;
using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Bson;

namespace Netronics
{
    public class Netronics
    {
        public enum Flag { Family, SocketType, ProtocolType, ServiceIPAddress, ServicePort, PacketEncoder, PacketDecoder }

        static protected AddressFamily family = AddressFamily.InterNetwork;
        static protected SocketType socketType = SocketType.Stream;
        static protected ProtocolType protocolType = ProtocolType.Tcp;
        static protected IPAddress addr = IPAddress.Any;
        static protected int port = 0;

        static protected PacketEncoder packetEncoder = new BSONEncoder();
        static protected PacketDecoder packetDecoder = new BSONDecoder();

        static protected Serivce oSerivce;
        static protected Socket oSocket;

        static protected Dictionary<string, LinkedList<Serivce>> globalSerivceList;

        static public Serivce serivce { set { Netronics.oSerivce = value; } get { return Netronics.oSerivce; } }

        static public void setFlag(Flag flag, object value)
        {
            switch (flag)
            {
                case Flag.Family:
                    if (value.GetType() != typeof(AddressFamily)) break;
                    Netronics.family = (AddressFamily)value;
                    break;
                case Flag.SocketType:
                    if (value.GetType() != typeof(SocketType)) break;
                    Netronics.socketType = (SocketType)value;
                    break;
                case Flag.ProtocolType:
                    if (value.GetType() != typeof(ProtocolType)) break;
                    Netronics.protocolType = (ProtocolType)value;
                    break;
                case Flag.ServiceIPAddress:
                    if (value.GetType() == typeof(IPAddress))
                        Netronics.addr = (IPAddress)value;
                    if (value.GetType() == typeof(string))
                        Netronics.addr = IPAddress.Parse((string)value);
                    break;
                case Flag.ServicePort:
                    if (value.GetType() != typeof(int)) break;
                    Netronics.port = (int)value;
                    break;
                case Flag.PacketEncoder:
                    if (value.GetType() != typeof(PacketEncoder)) break;
                    Netronics.packetEncoder = (PacketEncoder)value;
                    break;
                case Flag.PacketDecoder:
                    if (value.GetType() != typeof(PacketDecoder)) break;
                    Netronics.packetDecoder = (PacketDecoder)value;
                    break;
            }
        }

        static protected PacketEncoder getPacketEncoder()
        {
            return Netronics.packetEncoder;
        }
        static protected PacketDecoder getPacketDecoder()
        {
            return Netronics.packetDecoder;
        }

        static public void start()
        {
            if (Netronics.serivce == null)
                return;

            Netronics.initSerivceList();

            Netronics.initSocket();

            Netronics.serivce.start();
        }

        static protected void initSerivceList()
        {
            Netronics.globalSerivceList = new Dictionary<string, LinkedList<Serivce>>();
            LinkedList<Serivce> mySerivceType = new System.Collections.Generic.LinkedList<Serivce>();
            mySerivceType.AddFirst(Netronics.serivce);
            Netronics.globalSerivceList.Add(Netronics.serivce.getSerivceName(), mySerivceType);
        }

        static protected void initSocket()
        {
            Netronics.oSocket = new System.Net.Sockets.Socket(Netronics.family, Netronics.socketType, Netronics.protocolType);
            Netronics.oSocket.Bind(new IPEndPoint(Netronics.addr, Netronics.port));
            Netronics.oSocket.BeginAccept(new AsyncCallback(Netronics.acceptCallback), null);
        }

        static protected void startSocket()
        {
            Netronics.oSocket.Listen(50);
        }

        static protected void acceptCallback(IAsyncResult ar)
        {
            RemoteSerivce newSerivce = new RemoteSerivce(Netronics.oSocket.EndAccept(ar));
            Netronics.oSocket.BeginAccept(new AsyncCallback(Netronics.acceptCallback), null);

            newSerivce.getSocket().BeginReceive(newSerivce.getSocketBuffer(), 0, 512, SocketFlags.None, Netronics.readCallback, newSerivce);
        }

        static protected void readCallback(IAsyncResult ar)
        {
            RemoteSerivce serivce = (RemoteSerivce)ar.AsyncState;
            int len = serivce.getSocket().EndReceive(ar);
            serivce.getPacketBuffer().write(serivce.getSocketBuffer(), 0, len);
            dynamic packet = Netronics.getPacketDecoder().decode(serivce.getPacketBuffer());
            serivce.getSocket().BeginReceive(serivce.getSocketBuffer(), 0, 512, SocketFlags.None, Netronics.readCallback, serivce);

            if (packet == null || packet.type.GetType() != typeof(string))
                return;

            Netronics.processingPacket(serivce, packet);
        }

        static protected void processingPacket(RemoteSerivce serivce, dynamic packet)
        {
            if (((string)packet.type) != "Netronics")
            {
                Netronics.processingJobPacket(serivce, packet);
                return;
            }

            switch ((string)packet.type)
            {
                case "ping":
                    dynamic data = new JObject();
                    data.type = "pong";
                    //여기서 패킷전송
                    break;
            }
        }

        static protected void processingJobPacket(RemoteSerivce serivce, dynamic packet)
        {
            Job job = new Job(packet.serivce);
            job.group = packet.netronics.group;
            job.message = packet.netronics.message;
            job.setReceiver();

            Netronics.serivce.processingJob(serivce, job);
        }
        

        static public void stop()
        {
            serivce.stop();
        }

        static public void processingJob(Job job)
        {
            LinkedList<Serivce> serivceList;

            lock (Netronics.globalSerivceList)
            {
                serivceList = Netronics.globalSerivceList[job.getSerivceName()];
            }

            if (serivceList == null)
            {
                job.callFail();
                return;
            }

            lock (serivceList)
            {
            }
        }
    }
}
