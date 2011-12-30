using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Netronics;

namespace Test
{
    [TestClass]
    public class PacketBufferTest
    {
        [TestMethod]
        public void PacketBufferTest1()
        {
            PacketBuffer buffer = new PacketBuffer();
            buffer.Dispose();
            try
            {

                buffer.write(1);
            }
            catch (Exception) { return; }
            throw new Exception("Ssibal");
        }
        [TestMethod]
        public void PacketBufferTest2()
        {
            PacketBuffer buffer = new PacketBuffer();
            buffer.write(1);
            buffer.write(0);
            buffer.write(0);
            buffer.write(5);

            buffer.beginBufferIndex();
            uint m = 0;
            for (int i = 0; i < 4; i++)
                m += buffer.readUInt32();
            if (m != 6) { throw new Exception("PacketBuffer unji"); }

        }
    }
}
