using System;
using System.Net;
using System.Net.Sockets;

namespace Netronics
{
    public class Netronics
    {
        #region Flag enum

        public enum Flag
        {
            Family,
            SocketType,
            ProtocolType,
            ServiceIPAddress,
            ServicePort,
            PacketEncoder,
            PacketDecoder
        }

        #endregion

        private static AddressFamily family = AddressFamily.InterNetwork;
        private static SocketType socketType = SocketType.Stream;
        private static ProtocolType protocolType = ProtocolType.Tcp;
        private static IPAddress addr = IPAddress.Any;
        private static int port;

        private static PacketEncoder packetEncoder = new BSONEncoder();
        private static PacketDecoder packetDecoder = new BSONDecoder();

        protected static Service oService;
        protected static Socket oSocket;

        public static Service service
        {
            set { oService = value; }
            get { return oService; }
        }

        public static void setFlag(Flag flag, object value)
        {
            switch (flag)
            {
                case Flag.Family:
                    if (value.GetType() != typeof (AddressFamily)) break;
                    family = (AddressFamily) value;
                    break;
                case Flag.SocketType:
                    if (value.GetType() != typeof (SocketType)) break;
                    socketType = (SocketType) value;
                    break;
                case Flag.ProtocolType:
                    if (value.GetType() != typeof (ProtocolType)) break;
                    protocolType = (ProtocolType) value;
                    break;
                case Flag.ServiceIPAddress:
                    if (value.GetType() == typeof (IPAddress))
                        addr = (IPAddress) value;
                    if (value.GetType() == typeof (string))
                        addr = IPAddress.Parse((string) value);
                    break;
                case Flag.ServicePort:
                    if (value.GetType() != typeof (int)) break;
                    port = (int) value;
                    break;
                case Flag.PacketEncoder:
                    if (value.GetType() != typeof (PacketEncoder)) break;
                    packetEncoder = (PacketEncoder) value;
                    break;
                case Flag.PacketDecoder:
                    if (value.GetType() != typeof (PacketDecoder)) break;
                    packetDecoder = (PacketDecoder) value;
                    break;
            }
        }

        protected static PacketEncoder getPacketEncoder()
        {
            return packetEncoder;
        }

        protected static PacketDecoder getPacketDecoder()
        {
            return packetDecoder;
        }

        public static void start()
        {
            if (service == null)
                return;

            PacketProcessor.init(service);
            initSocket();
            service.init();

            startSocket();
            service.start();
        }

        protected static void initSocket()
        {
            oSocket = new Socket(family, socketType, protocolType);
            oSocket.Bind(new IPEndPoint(addr, port));
        }

        protected static void startSocket()
        {
            oSocket.Listen(50);
            oSocket.BeginAccept(acceptCallback, null);
        }

        protected static void acceptCallback(IAsyncResult ar)
        {
            new RemoteService(oSocket.EndAccept(ar), getPacketEncoder(), getPacketDecoder()).
                processingJob(service, ServiceJob.serviceInfo(service));
            oSocket.BeginAccept(acceptCallback, null);
        }

        public static void stop()
        {
            service.stop();
        }

        public static void processingJob(Job job)
        {
            PacketProcessor.processingJob(job);
        }
    }
}