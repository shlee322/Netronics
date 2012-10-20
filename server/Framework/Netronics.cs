using System;
using System.Net;
using System.Net.Sockets;
using Netronics.Channel.Channel;
using Netronics.Event;

namespace Netronics
{
    /// <summary>
    /// Netronics Framework
    /// </summary>
    public class Netronics
    {
        protected readonly IProperties Properties;
        protected Socket Socket;

        static Netronics()
        {
            Scheduler.GetThreadCount(); //스케줄러 활성화를 위해 한번 호출
        }

        /// <summary>
        /// Netronics를 생성한다
        /// </summary>
        /// <param name="properties">속성값</param>
        public Netronics(IProperties properties)
        {
            Properties = properties;
        }

        /// <summary>
        /// Netronics 를 시작하는 메소드
        /// </summary>
        /// <returns>시작된 Netronics</returns>
        public virtual Netronics Start()
        {
            if (Properties == null)
                return this;

            InitSocket();
            StartSocket();
            Properties.OnStartEvent(this, new StartEventArgs(Socket));

            return this;
        }

        /// <summary>
        /// Netronics를 중지하는 메소드
        /// </summary>
        /// <returns>중지된 Netronics</returns>
        public virtual Netronics Stop()
        {
            if (Properties != null)
                Properties.OnStopEvent(this, new EventArgs());
            Socket.Dispose();
            Socket = null;
            return this;
        }

        /// <summary>
        /// 소켓 초기화를 하는 메소드
        /// </summary>
        protected virtual void InitSocket()
        {
            Socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        }

        /// <summary>
        /// 소켓을 활성화 하는 메소드
        /// </summary>
        protected virtual void StartSocket()
        {
            Socket.Bind(Properties.GetIPEndPoint());
            Socket.Listen(50);
            Socket.BeginAccept(AcceptCallback, null);
        }

        /// <summary>
        /// Netronics에 사용되는 IPEndPoint를 반환하는 메소드
        /// </summary>
        /// <returns>사용되는 IPEndPoint</returns>
        public virtual IPEndPoint GetEndIPPoint()
        {
            return (IPEndPoint) Socket.LocalEndPoint;
        }

        protected virtual void AcceptCallback(IAsyncResult ar)
        {
            if (Socket == null)
                return;
            var channel = Properties.GetChannelPipe().CreateChannel(this, Socket.EndAccept(ar));
            if (channel != null)
                AddChannel(channel).Connect();
            Socket.BeginAccept(AcceptCallback, null);
        }

        /// <summary>
        /// 새로운 Channel을 추가하는 메소드
        /// </summary>
        /// <param name="channel">추가할 Channel</param>
        /// <returns>추가된 Channel</returns>
        public virtual IChannel AddChannel(IChannel channel)
        {
            return channel;
        }

        /// <summary>
        /// 새로운 Socket를 Channel로 변환하고 추가하는 메소드
        /// </summary>
        /// <param name="socket">추가할 Socket</param>
        /// <returns>추가된 Channel</returns>
        public virtual IChannel AddSocket(Socket socket)
        {
            return AddChannel(Properties.GetChannelPipe().CreateChannel(this, socket));
        }
    }
}