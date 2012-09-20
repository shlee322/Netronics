using System;
using System.Net;
using Netronics.Channel;
using Netronics.Event;

namespace Netronics
{
    /// <summary>
    /// 간단하게 Properties를 지정하기 위한 클래스
    /// </summary>
    public class Properties : IProperties
    {
        protected IChannelPipe ChannelPipe = new ChannelPipe();
        protected IPEndPoint IpEndPoint = new IPEndPoint(IPAddress.Any, 0);

        protected Properties()
        {
        }

        #region IProperties Members

        public void OnStartEvent(Netronics netronics, StartEventArgs eventArgs)
        {
            if (StartEvent != null)
                StartEvent(netronics, eventArgs);
        }

        public void OnStopEvent(Netronics netronics, EventArgs eventArgs)
        {
            if (StopEvent != null)
                StopEvent(netronics, eventArgs);
        }

        public IPEndPoint GetIPEndPoint()
        {
            return IpEndPoint;
        }

        public IChannelPipe GetChannelPipe()
        {
            return ChannelPipe;
        }

        #endregion

        protected event EventHandler<StartEventArgs> StartEvent;
        protected event EventHandler StopEvent;

        /// <summary>
        /// 새로운 Properties를 생성하는 메소드
        /// </summary>
        /// <param name="ipEndPoint">사용할 IPEndPoint</param>
        /// <param name="pipe">사용할 ChannelPipe</param>
        /// <returns>생성된 Properties 객체</returns>
        public static Properties CreateProperties(IPEndPoint ipEndPoint, IChannelPipe pipe)
        {
            var properties = new Properties();
            properties.SetIpEndPoint(ipEndPoint);
            properties.ChannelPipe = pipe;
            return properties;
        }

        public Properties SetIpEndPoint(IPEndPoint endPoint)
        {
            IpEndPoint = endPoint;
            return this;
        }
    }
}