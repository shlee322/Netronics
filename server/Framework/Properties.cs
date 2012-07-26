using System;
using System.Net;
using Netronics.Channel;

namespace Netronics
{
    public class Properties : IProperties
    {
        protected IChannelPipe ChannelPipe = new ChannelPipe();
        protected IPEndPoint IpEndPoint = new IPEndPoint(IPAddress.Any, 0);

        protected event EventHandler StartEvent;
        protected event EventHandler StopEvent;

        public static Properties CreateProperties(IPEndPoint ipEndPoint, ChannelPipe pipe)
        {
            var properties = new Properties();
            properties.SetIpEndPoint(ipEndPoint);
            properties.ChannelPipe = pipe;
            return properties;
        }

        public void OnStartEvent(Netronics netronics, EventArgs eventArgs)
        {
            if (StartEvent != null)
                StartEvent(netronics, eventArgs);
        }

        public void OnStopEvent(Netronics netronics, EventArgs eventArgs)
        {
            if (StopEvent != null)
                StopEvent(netronics, eventArgs);
        }

        public Properties SetIpEndPoint(IPEndPoint endPoint)
        {
            IpEndPoint = endPoint;
            return this;
        }

        public Properties SetChannelFactoryOption(Action<IChannelPipe> action)
        {
            action(GetChannelPipe());
            return this;
        }

        public IPEndPoint GetIPEndPoint()
        {
            return IpEndPoint;
        }

        public IChannelPipe GetChannelPipe()
        {
            return ChannelPipe;
        }
    }
}