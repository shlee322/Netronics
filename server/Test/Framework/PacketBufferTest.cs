using System;
using NUnit.Framework;
using Netronics;

namespace Framework
{
    [TestFixture()]
    public class PacketBufferTest
    {
        [Test()]
        public void PacketBufferTest1()
        {
            PacketBuffer buffer = new PacketBuffer();
            buffer.Dispose();
            try
            {

                buffer.Write(1);
            }
            catch (Exception) { return; }
            throw new Exception("Ssibal");
        }
        [Test()]
        public void PacketBufferTest2()
        {
            PacketBuffer buffer = new PacketBuffer();
            buffer.Write(1);
            buffer.Write(0);
            buffer.Write(0);
            buffer.Write(5);

            buffer.BeginBufferIndex();
            uint m = 0;
            for (int i = 0; i < 4; i++)
                m += buffer.ReadUInt32();
            if (m != 6) { throw new Exception("PacketBuffer unji"); }

        }
    }
}
