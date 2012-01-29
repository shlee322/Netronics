using System;
using System.Net;
using Netronics.Channel;

namespace Netronics
{
    public class Properties
    {
        public IPEndPoint IpEndPoint = new IPEndPoint(IPAddress.Any, 0);
        public IChannelFactory ChannelFactory = new BasicChannelFactory();

        public event EventHandler StartEvent;
        public event EventHandler StopEvent;

        public void OnStartEvent(Netronics netronics, EventArgs eventArgs)
        {
            StartEvent(netronics, eventArgs);
        }

        public void OnStopEvent(Netronics netronics, EventArgs eventArgs)
        {
            StopEvent(netronics, eventArgs);
        }
    }
}
