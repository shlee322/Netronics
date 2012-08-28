using System;
using System.IO;
using System.Runtime.InteropServices;

namespace Netronics
{
    public class PacketBuffer : IDisposable
    {
        private readonly byte[] _buf = new byte[1024];

        protected bool Disposed;
        private MemoryStream _buffer = new MemoryStream();

        #region IDisposable Members

        public void Dispose()
        {
            Dispose(true);

            GC.SuppressFinalize(this);
        }

        #endregion

        public bool IsDisposed()
        {
            return Disposed;
        }

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

        public Stream GetStream()
        {
            return _buffer;
        }

        public byte[] GetBytes()
        {
            return _buffer.ToArray();
        }

        public long AvailableBytes()
        {
            return _buffer.Length - _buffer.Position;
        }

        public void BeginBufferIndex()
        {
            _buffer.Position = 0;
        }

        public void SetPosition(int p)
        {
            _buffer.Position = p;
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
            _buffer.Position = _buffer.Length;
            _buffer.Write(buffer, offset, count);
        }

        public void Write(object value)
        {
            if (value is Stream)
                WriteStream((Stream)value);
            else if (value is UInt16)
                WriteUInt16((UInt16)value);
            else if (value is UInt32)
                WriteUInt32((UInt32)value);
            else if (value is UInt64)
                WriteUInt64((UInt64)value);
            else if (value is byte)
                WriteByte((byte)value);
            else if (value is Int16)
                WriteInt16((Int16)value);
            else if (value is Int32)
                WriteInt32((Int32)value);
            else if (value is Int64)
                WriteInt64((Int64)value);
            else if (value is byte[])
                WriteBytes((byte[]) value);
        }

        public void WriteBytes(byte[] value)
        {
            Write(value, 0, value.Length);
        }

        public void WriteStream(Stream stream)
        {
            _buffer.Position = _buffer.Length;
            stream.CopyTo(_buffer);
        }
        public void WriteUInt16(UInt16 value)
        {
            byte[] buffer = BitConverter.GetBytes(value);
            Array.Reverse(buffer);
            Write(buffer, 0, 2);
        }
        public void WriteUInt32(UInt32 value)
        {
            byte[] buffer = BitConverter.GetBytes(value);
            Array.Reverse(buffer);
            Write(buffer, 0, 4);
        }
        public void WriteUInt64(UInt64 value)
        {
            byte[] buffer = BitConverter.GetBytes(value);
            Array.Reverse(buffer);
            Write(buffer, 0, 8);
        }

        public void WriteByte(byte value)
        {
            byte[] buffer = BitConverter.GetBytes(value);
            Write(buffer, 0, 1);
        }

        public void WriteInt16(Int16 value)
        {
            byte[] buffer = BitConverter.GetBytes(value);
            Array.Reverse(buffer);
            Write(buffer, 0, 2);
        }

        public void WriteInt32(Int32 value)
        {
            byte[] buffer = BitConverter.GetBytes(value);
            Array.Reverse(buffer);
            Write(buffer, 0, 4);
        }

        public void WriteInt64(Int64 value)
        {
            byte[] buffer = BitConverter.GetBytes(value);
            Array.Reverse(buffer);
            Write(buffer, 0, 8);
        }

        public int Read(byte[] buffer, int offset, int count)
        {
            int len = _buffer.Read(buffer, offset, count);
            if(len != count)
                throw new PacketLengthException();
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
            Array.Reverse(int16Data);
            return BitConverter.ToInt16(int16Data, 0);
        }

        public Int32 ReadInt32()
        {
            var int32Data = new byte[4];
            Read(int32Data, 0, 4);
            Array.Reverse(int32Data);
            return BitConverter.ToInt32(int32Data, 0);
        }

        public Int64 ReadInt64()
        {
            var int64Data = new byte[8];
            Read(int64Data, 0, 8);
            Array.Reverse(int64Data);
            return BitConverter.ToInt64(int64Data, 0);
        }

        public UInt16 ReadUInt16()
        {
            var uint16Data = new byte[2];
            Read(uint16Data, 0, 2);
            Array.Reverse(uint16Data);
            return BitConverter.ToUInt16(uint16Data, 0);
        }

        public UInt32 ReadUInt32()
        {
            var uint32Data = new byte[4];
            Read(uint32Data, 0, 4);
            Array.Reverse(uint32Data);
            return BitConverter.ToUInt32(uint32Data, 0);
        }

        public UInt64 ReadUInt64()
        {
            var uint64Data = new byte[8];
            Read(uint64Data, 0, 8);
            Array.Reverse(uint64Data);
            return BitConverter.ToUInt64(uint64Data, 0);
        }

        public byte ReadByte()
        {
            var byteData = new byte[1];
            Read(byteData, 0, 1);
            return byteData[0];
        }

        public string ReadString(int len)
        {
            return System.Text.Encoding.UTF8.GetString(ReadBytes(len));
        }

        public long FindBytes(byte[] q)
        {
            long p = _buffer.Position;

            var bytes = new byte[q.Length*2];
            if (Read(bytes, 0, q.Length) != q.Length)
                return -1;

            int len = 0;
            bool find = true;

            for (int x = 0; x < q.Length; x++)
            {
                if (bytes[x] != q[x])
                {
                    find = false;
                    break;
                }
            }

            if (find)
            {
                long r = _buffer.Position - len - q.Length;
                _buffer.Position = p;
                return r - _buffer.Position;
            }

            find = true;

            long temp = 0;
            while ((len = Read(bytes, q.Length, q.Length)) > 0)
            {
                temp = len;
                for (int i = 0; i <= q.Length; i++)
                {
                    for (int x = 0; x < q.Length; x++)
                    {
                        if (bytes[i+x] != q[x])
                        {
                            find = false;
                            break;
                        }
                    }

                    if (find)
                    {
                        long r = _buffer.Position - len + i - q.Length;
                        _buffer.Position = p;
                        return r - _buffer.Position;
                    }
                    find = true;
                }
                if (len != q.Length)
                    break;
                Array.Copy(bytes, q.Length, bytes, 0, q.Length);
               
            }
            _buffer.Position = p;
            return -1;
        }

        public string ReadLine()
        {
            long len = FindBytes(new byte[] {13, 10});
            if (len == -1)
                return null;
            string r = ReadString((int) len);
            ReadBytes(2);
            return r;
        }
    }
}