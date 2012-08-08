using System;
using System.Net;
using Netronics.Channel;
using Netronics.Event;

namespace Netronics
{
    public interface IProperties
    {
        void OnStartEvent(Netronics netronics, StartEventArgs eventArgs);
        void OnStopEvent(Netronics netronics, EventArgs eventArgs);
        IPEndPoint GetIPEndPoint();
        IChannelPipe GetChannelPipe();
    }
}