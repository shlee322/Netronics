using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace Netronics
{
    class PacketBuffer
    {
        MemoryStream buffer = new MemoryStream();
        MemoryStream usedBuffer;
        
        public long legibleBytes()
        {
            return buffer.Length;
        }

        public void beginBufferIndex()
        {
            if (usedBuffer == null)
                usedBuffer = new MemoryStream();
        }

        public void endBufferIndex()
        {
            usedBuffer = null;
        }

        public void resetBufferIndex()
        {
            buffer.WriteTo(usedBuffer);
            buffer = usedBuffer;
            usedBuffer = null;
        }

        public int read(byte[] buffer, int offset, int count)
        {
            int len = this.buffer.Read(buffer, offset, count);
            if (usedBuffer != null)
                usedBuffer.Write(buffer, offset, count);
            return len;
        }

        public void readBytes(byte[] buffer)
        {
            this.read(buffer, 0, buffer.Length);
        }

        public byte[] readBytes(int length)
        {
            byte[] buffer = new byte[length];
            this.readBytes(buffer);
            return buffer;
        }

        public Int16 readInt16()
        {
            byte[] int16Data = new byte[2];
            this.read(int16Data, 0, 2);
            return Convert.ToInt16(int16Data);
        }

        public Int32 readInt32()
        {
            byte[] int32Data = new byte[4];
            this.read(int32Data, 0, 4);
            return Convert.ToInt32(int32Data);
        }

        public Int64 readInt64()
        {
            byte[] int64Data = new byte[8];
            this.read(int64Data, 0, 8);
            return Convert.ToInt64(int64Data);
        }

        public UInt16 readUInt16()
        {
            byte[] uint16Data = new byte[2];
            this.read(uint16Data, 0, 2);
            return Convert.ToUInt16(uint16Data);
        }

        public UInt32 readUInt32()
        {
            byte[] uint32Data = new byte[4];
            this.read(uint32Data, 0, 4);
            return Convert.ToUInt32(uint32Data);
        }

        public UInt64 readUInt64()
        {
            byte[] uint64Data = new byte[8];
            this.read(uint64Data, 0, 8);
            return Convert.ToUInt64(uint64Data);
        }

        public byte readByte()
        {
            byte[] byteData = new byte[1];
            this.read(byteData, 0, 1);
            return byteData[0];
        }
    }
}
