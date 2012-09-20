using System;

namespace Netronics
{
    /// <summary>
    /// PacketBuffer에서 읽을 수 있는 Packet의 길이가 부족할때 발생한는 Exception
    /// </summary>
    internal class PacketLengthException : Exception
    {
    }
}