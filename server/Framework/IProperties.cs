using System;
using System.Net;
using Netronics.Channel;
using Netronics.Event;

namespace Netronics
{
    public interface IProperties
    {
        /// <summary>
        /// Netronics�� ���ۉ����� ȣ��Ǵ� �޼ҵ�
        /// </summary>
        /// <param name="netronics">���۵� Netronics ��ü</param>
        /// <param name="eventArgs">StartEventArgs</param>
        void OnStartEvent(Netronics netronics, StartEventArgs eventArgs);

        /// <summary>
        /// Netronics�� �����Ǿ����� ȣ��Ǵ� �޼ҵ�
        /// </summary>
        /// <param name="netronics">���۵� Netronics ��ü</param>
        /// <param name="eventArgs">eventArgs</param>
        void OnStopEvent(Netronics netronics, EventArgs eventArgs);

        /// <summary>
        /// Netronics���� ����� IPEndPoint�� ��ȯ�ϴ� �޼ҵ�
        /// </summary>
        /// <returns>Netronics���� ����� IPEndPoint</returns>
        IPEndPoint GetIPEndPoint();

        /// <summary>
        /// Netronics���� ����� ChannelPipe�� ��ȯ�ϴ� �޼ҵ�
        /// </summary>
        /// <returns>Netronics���� ����� ChannelPipe</returns>
        IChannelPipe GetChannelPipe();
    }
}