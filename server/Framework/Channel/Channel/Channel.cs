using System.Threading;
using Netronics.Protocol;

namespace Netronics.Channel.Channel
{
    public class Channel : IKeepProtocolChannel, IKeepHandlerChannel, IKeepParallelChannel
    {
        private readonly PacketBuffer _packetBuffer = new PacketBuffer();
        private IProtocol _protocol;
        private IChannelHandler _handler;
        private bool _parallel;

        protected bool ReceivePacket(byte[] buffer, int len)
        {
            lock (_packetBuffer)
            {
                if (_packetBuffer.IsDisposed())
                    return false;
                _packetBuffer.Write(buffer, 0, len);
            }
            ThreadPool.QueueUserWorkItem((o) => Receive());
            return true;
        }

        protected virtual void Disconnected()
        {
            lock (_packetBuffer)
            {
                _packetBuffer.Dispose();
            }
        }

        private void Receive()
        {
            dynamic message;

            lock (_packetBuffer)
            {
                if (_packetBuffer.IsDisposed())
                    return;
                message = GetProtocol().GetDecoder().Decode(this, _packetBuffer);
            }

            if (message == null)
            {
                BeginReceive();
                return;
            }

            if (GetParallel())
            {
                ThreadPool.QueueUserWorkItem((o) => GetHandler().MessageReceive(this, message));
                ThreadPool.QueueUserWorkItem((o) => Receive());
            }
            else
            {
                ThreadPool.QueueUserWorkItem((o) =>
                {
                    GetHandler().MessageReceive(this, message);
                    ThreadPool.QueueUserWorkItem((s) => Receive());
                });
            }
        }
        public virtual IProtocol SetProtocol(IProtocol protocol)
        {
            _protocol = protocol;
            return protocol;
        }

        public virtual IProtocol GetProtocol()
        {
            return _protocol;
        }

        public virtual IChannelHandler SetHandler(IChannelHandler handler)
        {
            _handler = handler;
            return handler;
        }

        public virtual IChannelHandler GetHandler()
        {
            return _handler;
        }

        public bool SetParallel(bool parallel)
        {
            _parallel = parallel;
            return parallel;
        }

        protected virtual bool GetParallel()
        {
            return _parallel;
        }

        public virtual void Connect()
        {
            if (GetHandler() != null)
                GetHandler().Connected(this);

            BeginReceive();
        }

        protected virtual void BeginReceive()
        {
            
        }

        public virtual void Disconnect()
        {
        }

        public virtual void SendMessage(dynamic message)
        {
        }

        public virtual object SetTag(object tag)
        {
            return null;
        }

        public virtual object GetTag()
        {
            return null;
        }
    }
}
