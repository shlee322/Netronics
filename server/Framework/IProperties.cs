using System;
using System.Net;
using Netronics.Channel;
using Netronics.Event;

namespace Netronics
{
    public interface IProperties
    {
        /// <summary>
        /// Netronics가 시작됬을때 호출되는 메소드
        /// </summary>
        /// <param name="netronics">시작된 Netronics 객체</param>
        /// <param name="eventArgs">StartEventArgs</param>
        void OnStartEvent(Netronics netronics, StartEventArgs eventArgs);

        /// <summary>
        /// Netronics가 중지되었을때 호출되는 메소드
        /// </summary>
        /// <param name="netronics">시작된 Netronics 객체</param>
        /// <param name="eventArgs">eventArgs</param>
        void OnStopEvent(Netronics netronics, EventArgs eventArgs);

        /// <summary>
        /// Netronics에서 사용할 IPEndPoint를 반환하는 메소드
        /// </summary>
        /// <returns>Netronics에서 사용할 IPEndPoint</returns>
        IPEndPoint GetIPEndPoint();

        /// <summary>
        /// Netronics에서 사용할 ChannelPipe를 반환하는 메소드
        /// </summary>
        /// <returns>Netronics에서 사용할 ChannelPipe</returns>
        IChannelPipe GetChannelPipe();
    }
}