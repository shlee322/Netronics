using System;
using System.Net;
using Netronics.Channel;

namespace Netronics
{
    public interface IProperties
    {
        void OnStartEvent(Netronics netronics, EventArgs eventArgs);
        void OnStopEvent(Netronics netronics, EventArgs eventArgs);
        IPEndPoint GetIPEndPoint();
        IChannelPipe GetChannelPipe();
    }
}