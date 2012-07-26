using System.Net.Sockets;
using Netronics.Channel.Channel;

namespace Netronics.Channel
{
    /// <summary>
    /// <see cref="Netronics"/>의 핵심 인터페이스인 <see cref="IChannel"/>의 생성을 담당하는 인터페이스.
    /// <see cref="IProperties"/>의 <see cref="IProperties.GetChannelPipe"/>에 의해 <see cref="Netronics"/>로 전달됩니다.
    /// </summary>
    public interface IChannelPipe
    {
        /// <summary>
        /// 채널을 생성하는 메서드
        /// </summary>
        /// <param name="netronics">생성을 요청한 <see cref="Netronics"/> 객체</param>
        /// <param name="socket">채널을 이루는 <see cref="Socket"/></param>
        /// <returns>생성된 채널</returns>
        IChannel CreateChannel(Netronics netronics, Socket socket);
    }
}