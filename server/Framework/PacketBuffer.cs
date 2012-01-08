using System;
using System.IO;

namespace Netronics
{
    public class PacketBuffer : IDisposable
    {
        private readonly byte[] _buf = new byte[1024];
        private MemoryStream _buffer = new MemoryStream();

        protected bool Disposed;

        #region IDisposable Members

        public void Dispose()
        {
            Dispose(true);

            GC.SuppressFinalize(this);
        }

        #endregion

        protected virtual void Dispose(bool disposing)
        {
            if (!Disposed)
            {
                if (disposing)
                {
                    _buffer.Dispose();
                }

                Disposed = true;
            }
        }

        public byte[] GetBytes()
        {
            return _buffer.ToArray();
        }

        public long LegibleBytes()
        {
            return _buffer.Length - _buffer.Position;
        }

        public void BeginBufferIndex()
        {
            _buffer.Position = 0;
        }

        public void EndBufferIndex()
        {
            MemoryStream old = _buffer;
            _buffer = new MemoryStream();

            int len;
            while ((len = old.Read(_buf, 0, 1024)) > 0)
                _buffer.Write(_buf, 0, len);
            old.Dispose();
        }

        public void ResetBufferIndex()
        {
            _buffer.Position = 0;
        }

        public void Write(byte[] buffer, int offset, int count)
        {
            this._buffer.Position = this._buffer.Length;
            this._buffer.Write(buffer, offset, count);
        }

        public void Write(Stream stream)
        {
            _buffer.Position = _buffer.Length;
            stream.CopyTo(_buffer);
        }

        public void Write(UInt32 value)
        {
            Write(BitConverter.GetBytes(value), 0, 4);
        }

        public int Read(byte[] buffer, int offset, int count)
        {
            int len = this._buffer.Read(buffer, offset, count);
            return len;
        }

        public void ReadBytes(byte[] buffer)
        {
            Read(buffer, 0, buffer.Length);
        }

        public byte[] ReadBytes(int length)
        {
            var buffer = new byte[length];
            ReadBytes(buffer);
            return buffer;
        }

        public Int16 ReadInt16()
        {
            var int16Data = new byte[2];
            Read(int16Data, 0, 2);
            return BitConverter.ToInt16(int16Data, 0);
        }

        public Int32 ReadInt32()
        {
            var int32Data = new byte[4];
            Read(int32Data, 0, 4);
            return BitConverter.ToInt32(int32Data, 0);
        }

        public Int64 ReadInt64()
        {
            var int64Data = new byte[8];
            Read(int64Data, 0, 8);
            return BitConverter.ToInt64(int64Data, 0);
        }

        public UInt16 ReadUInt16()
        {
            var uint16Data = new byte[2];
            Read(uint16Data, 0, 2);
            return BitConverter.ToUInt16(uint16Data, 0);
        }

        public UInt32 ReadUInt32()
        {
            var uint32Data = new byte[4];
            Read(uint32Data, 0, 4);
            return BitConverter.ToUInt32(uint32Data, 0);
        }

        public UInt64 ReadUInt64()
        {
            var uint64Data = new byte[8];
            Read(uint64Data, 0, 8);
            return BitConverter.ToUInt64(uint64Data, 0);
        }

        public byte ReadByte()
        {
            var byteData = new byte[1];
            Read(byteData, 0, 1);
            return byteData[0];
        }
    }
}