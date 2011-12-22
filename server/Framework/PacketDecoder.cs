using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Netronics
{
    public interface PacketDecoder
    {
        /// <summary>
        /// 패킷을 디코딩하는 메서드
        /// </summary>
        /// <param name="buffer">PacketBuffer</param>
        /// <returns>패킷 메시지 객체</returns>
        dynamic decode(PacketBuffer buffer);
    }
}
