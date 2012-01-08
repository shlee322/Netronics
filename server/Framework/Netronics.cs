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
            ServiceIpAddress,
            ServicePort,
            PacketEncoder,
            PacketDecoder
        }

        #endregion

        private static AddressFamily _family = AddressFamily.InterNetwork;
        private static SocketType _socketType = SocketType.Stream;
        private static ProtocolType _protocolType = ProtocolType.Tcp;
        private static IPAddress _addr = IPAddress.Any;
        private static int _port;

        private static IPacketEncoder _packetEncoder = new BsonEncoder();
        private static IPacketDecoder _packetDecoder = new BsonDecoder();

        protected static Service _Service;
        protected static Socket _Socket;

        public static Service Service
        {
            set { _Service = value; }
            get { return _Service; }
        }

        public static void SetFlag(Flag flag, object value)
        {
            switch (flag)
            {
                case Flag.Family:
                    if (value is AddressFamily) break;
                    _family = (AddressFamily) value;
                    break;
                case Flag.SocketType:
                    if (value is SocketType) break;
                    _socketType = (SocketType) value;
                    break;
                case Flag.ProtocolType:
                    if (value is ProtocolType) break;
                    _protocolType = (ProtocolType) value;
                    break;
                case Flag.ServiceIpAddress:
                    if (value is IPAddress)
                        _addr = (IPAddress) value;
                    if (value is string)
                        _addr = IPAddress.Parse((string) value);
                    break;
                case Flag.ServicePort:
                    if (value is int) break;
                    _port = (int) value;
                    break;
                case Flag.PacketEncoder:
                    if (value is IPacketEncoder) break;
                    _packetEncoder = (IPacketEncoder) value;
                    break;
                case Flag.PacketDecoder:
                    if (value is IPacketDecoder) break;
                    _packetDecoder = (IPacketDecoder) value;
                    break;
            }
        }

        protected static IPacketEncoder GetPacketEncoder()
        {
            return _packetEncoder;
        }

        protected static IPacketDecoder GetPacketDecoder()
        {
            return _packetDecoder;
        }

        public static void Start()
        {
            if (Service == null)
                return;

            PacketProcessor.Init(Service);
            InitSocket();
            Service.Init();

            StartSocket();
            Service.Start();
        }

        protected static void InitSocket()
        {
            _Socket = new Socket(_family, _socketType, _protocolType);
            _Socket.Bind(new IPEndPoint(_addr, _port));
        }

        protected static void StartSocket()
        {
            _Socket.Listen(50);
            _Socket.BeginAccept(AcceptCallback, null);
        }

        protected static void AcceptCallback(IAsyncResult ar)
        {
            new RemoteService(_Socket.EndAccept(ar), GetPacketEncoder(), GetPacketDecoder()).
                ProcessingJob(Service, ServiceJob.ServiceInfo(Service));
            _Socket.BeginAccept(AcceptCallback, null);
        }

        public static void Stop()
        {
            Service.Stop();
        }

        public static void ProcessingJob(Job job)
        {
            PacketProcessor.ProcessingJob(job);
        }
    }
}