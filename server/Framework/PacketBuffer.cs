using System;
using System.IO;

namespace Netronics
{
    public class PacketBuffer : IDisposable
    {
        private readonly byte[] buf = new byte[1024];
        private MemoryStream buffer = new MemoryStream();

        protected bool disposed;

        #region IDisposable Members

        public void Dispose()
        {
            Dispose(true);

            GC.SuppressFinalize(this);
        }

        #endregion

        protected virtual void Dispose(bool disposing)
        {
            if (!disposed)
            {
                if (disposing)
                {
                    buffer.Dispose();
                }

                disposed = true;
            }
        }

        public byte[] getBytes()
        {
            return buffer.ToArray();
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
            MemoryStream old = buffer;
            buffer = new MemoryStream();

            int len;
            while ((len = old.Read(buf, 0, 1024)) > 0)
                buffer.Write(buf, 0, len);
            old.Dispose();
        }

        public void resetBufferIndex()
        {
            buffer.Position = 0;
        }

        public void write(byte[] buffer, int offset, int count)
        {
            this.buffer.Position = this.buffer.Length;
            this.buffer.Write(buffer, offset, count);
        }

        public void write(Stream stream)
        {
            buffer.Position = buffer.Length;
            stream.CopyTo(buffer);
        }

        public void write(UInt32 value)
        {
            write(BitConverter.GetBytes(value), 0, 4);
        }

        public int read(byte[] buffer, int offset, int count)
        {
            int len = this.buffer.Read(buffer, offset, count);
            return len;
        }

        public void readBytes(byte[] buffer)
        {
            read(buffer, 0, buffer.Length);
        }

        public byte[] readBytes(int length)
        {
            var buffer = new byte[length];
            readBytes(buffer);
            return buffer;
        }

        public Int16 readInt16()
        {
            var int16Data = new byte[2];
            read(int16Data, 0, 2);
            return BitConverter.ToInt16(int16Data, 0);
        }

        public Int32 readInt32()
        {
            var int32Data = new byte[4];
            read(int32Data, 0, 4);
            return BitConverter.ToInt32(int32Data, 0);
        }

        public Int64 readInt64()
        {
            var int64Data = new byte[8];
            read(int64Data, 0, 8);
            return BitConverter.ToInt64(int64Data, 0);
        }

        public UInt16 readUInt16()
        {
            var uint16Data = new byte[2];
            read(uint16Data, 0, 2);
            return BitConverter.ToUInt16(uint16Data, 0);
        }

        public UInt32 readUInt32()
        {
            var uint32Data = new byte[4];
            read(uint32Data, 0, 4);
            return BitConverter.ToUInt32(uint32Data, 0);
        }

        public UInt64 readUInt64()
        {
            var uint64Data = new byte[8];
            read(uint64Data, 0, 8);
            return BitConverter.ToUInt64(uint64Data, 0);
        }

        public byte readByte()
        {
            var byteData = new byte[1];
            read(byteData, 0, 1);
            return byteData[0];
        }
    }
}