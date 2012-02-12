using System;
using System.Net;
using Netronics.Channel;

namespace Netronics
{
    public class Properties
    {
        protected IChannelFactory ChannelFactory = new BasicChannelFactory();
        protected IPEndPoint IpEndPoint = new IPEndPoint(IPAddress.Any, 0);

        protected event EventHandler StartEvent;
        protected event EventHandler StopEvent;

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

        public Properties SetChannelFactoryOption(Action<IChannelFactory> action)
        {
            action(GetChannelFactory());
            return this;
        }

        public IPEndPoint GetIPEndPoint()
        {
            return IpEndPoint;
        }

        public IChannelFactory GetChannelFactory()
        {
            return ChannelFactory;
        }
    }
}