using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace Netronics
{
    public class PacketBuffer : IDisposable
    {
        MemoryStream buffer = new MemoryStream();

        protected bool disposed = false;

        public void Dispose()
        {
            this.Dispose(true);

            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    buffer.Dispose();
                }

                disposed = true;
            }
        }

        public MemoryStream getBufferStream()
        {
            return this.buffer;
        }

        public long legibleBytes()
        {
            return buffer.Length - buffer.Position;
        }

        public void beginBufferIndex()
        {
            buffer.Position = 0;
        }

        public void endBufferIndex()
        {
            buffer.Position = 0;
        }

        public void resetBufferIndex()
        {
            this.endBufferIndex();
        }

        public void write(byte[] buffer, int offset, int count)
        {
            long position = this.buffer.Position;
            this.buffer.Position = this.buffer.Length;
            this.buffer.Write(buffer, offset, count);
            this.buffer.Position = position;
        }

        public void write(UInt32 value)
        {
            this.write(BitConverter.GetBytes(value), 0, 4);
        }

        public int read(byte[] buffer, int offset, int count)
        {
            int len = this.buffer.Read(buffer, offset, count);
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
            return BitConverter.ToInt16(int16Data, 0);
        }

        public Int32 readInt32()
        {
            byte[] int32Data = new byte[4];
            this.read(int32Data, 0, 4);
            return BitConverter.ToInt32(int32Data, 0);
        }

        public Int64 readInt64()
        {
            byte[] int64Data = new byte[8];
            this.read(int64Data, 0, 8);
            return BitConverter.ToInt64(int64Data, 0);
        }

        public UInt16 readUInt16()
        {
            byte[] uint16Data = new byte[2];
            this.read(uint16Data, 0, 2);
            return BitConverter.ToUInt16(uint16Data, 0);
        }

        public UInt32 readUInt32()
        {
            byte[] uint32Data = new byte[4];
            this.read(uint32Data, 0, 4);
            return BitConverter.ToUInt32(uint32Data, 0);
        }

        public UInt64 readUInt64()
        {
            byte[] uint64Data = new byte[8];
            this.read(uint64Data, 0, 8);
            return BitConverter.ToUInt64(uint64Data, 0);
        }

        public byte readByte()
        {
            byte[] byteData = new byte[1];
            this.read(byteData, 0, 1);
            return byteData[0];
        }
    }
}
