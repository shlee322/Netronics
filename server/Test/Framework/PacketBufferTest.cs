using System;
using NUnit.Framework;

namespace Netronics.Test
{
    [TestFixture]
    public class PacketBufferTest
    {
        [Test]
        public void PacketBufferTest1()
        {
            PacketBuffer buffer = new PacketBuffer();
            buffer.Dispose();
            try
            {

                buffer.Write(1);
            }
            catch (Exception) { return; }
            throw new Exception("error");
        }

        [Test]
        public void PacketBufferInt16Test()
        {
            var buffer = new PacketBuffer();
            buffer.WriteInt16(1);
            buffer.WriteInt16(0);
            buffer.WriteInt16(0);
            buffer.WriteInt16(5);

            buffer.BeginBufferIndex();
            long m = 0;
            for (int i = 0; i < 4; i++)
                m += buffer.ReadInt16();
            if (m != 6)
            {
                throw new Exception("PacketBuffer error");
            }
        }

        [Test]
        public void PacketBufferInt32Test()
        {
            var buffer = new PacketBuffer();
            buffer.WriteInt32(1);
            buffer.WriteInt32(0);
            buffer.WriteInt32(0);
            buffer.WriteInt32(5);

            buffer.BeginBufferIndex();
            long m = 0;
            for (int i = 0; i < 4; i++)
                m += buffer.ReadInt32();
            if (m != 6)
            {
                throw new Exception("PacketBuffer error");
            }
        }

        [Test]
        public void PacketBufferInt64Test()
        {
            var buffer = new PacketBuffer();
            buffer.WriteInt64(1);
            buffer.WriteInt64(0);
            buffer.WriteInt64(0);
            buffer.WriteInt64(5);

            buffer.BeginBufferIndex();
            long m = 0;
            for (int i = 0; i < 4; i++)
                m += buffer.ReadInt64();
            if (m != 6)
            {
                throw new Exception("PacketBuffer error");
            }
        }

        [Test]
        public void PacketBufferUInt16Test()
        {
            var buffer = new PacketBuffer();
            buffer.WriteUInt16(1);
            buffer.WriteUInt16(0);
            buffer.WriteUInt16(0);
            buffer.WriteUInt16(5);

            buffer.BeginBufferIndex();
            long m = 0;
            for (int i = 0; i < 4; i++)
                m += buffer.ReadUInt16();
            if (m != 6)
            {
                throw new Exception("PacketBuffer error");
            }
        }

        [Test]
        public void PacketBufferUInt32Test()
        {
            var buffer = new PacketBuffer();
            buffer.WriteUInt32(1);
            buffer.WriteUInt32(0);
            buffer.WriteUInt32(0);
            buffer.WriteUInt32(5);

            buffer.BeginBufferIndex();
            long m = 0;
            for (int i = 0; i < 4; i++)
                m += buffer.ReadUInt32();
            if (m != 6)
            {
                throw new Exception("PacketBuffer error");
            }
        }

        [Test]
        public void PacketBufferUInt64Test()
        {
            var buffer = new PacketBuffer();
            buffer.WriteUInt64(1);
            buffer.WriteUInt64(0);
            buffer.WriteUInt64(0);
            buffer.WriteUInt64(5);

            buffer.BeginBufferIndex();
            ulong m = 0;
            for (int i = 0; i < 4; i++)
                m += buffer.ReadUInt64();
            if (m != 6)
            {
                throw new Exception("PacketBuffer error");
            }
        }

        [Test]
        public void PacketBufferByteTest()
        {
            var buffer = new PacketBuffer();
            buffer.WriteByte(1);
            buffer.WriteByte(0);
            buffer.WriteByte(0);
            buffer.WriteByte(5);

            buffer.BeginBufferIndex();

            if(buffer.ReadByte() != 1)
                throw new Exception("PacketBuffer error");

            if (buffer.ReadByte() != 0)
                throw new Exception("PacketBuffer error");

            if (buffer.ReadByte() != 0)
                throw new Exception("PacketBuffer error");

            if (buffer.ReadByte() != 5)
                throw new Exception("PacketBuffer error");
        }

        [Test]
        public void PacketBufferBytesTest()
        {
            var data = new byte[] {1, 0, 0, 5};
            var buffer = new PacketBuffer();
            buffer.WriteBytes(data);

            buffer.BeginBufferIndex();

            var temp = buffer.ReadBytes(4);
            for (int i = 0; i < temp.Length; i++)
            {
                if(temp[i] != data[i])
                    throw new Exception("PacketBuffer error");
            }
        }


    }
}
