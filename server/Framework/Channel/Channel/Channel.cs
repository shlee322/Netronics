using System;
using System.Threading;
using Netronics.Protocol;
using log4net;

namespace Netronics.Channel.Channel
{
    public class Channel : IKeepProtocolChannel, IKeepHandlerChannel, IKeepParallelChannel
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(Channel)); 

        private readonly PacketBuffer _packetBuffer = new PacketBuffer();
        private IProtocol _protocol;
        private IChannelHandler _handler;
        private bool _parallel;

        protected bool ReceivePacket(byte[] buffer, int len)
        {
            if (_packetBuffer.IsDisposed())
                return false;
            _packetBuffer.Write(buffer, 0, len);
            Receive();
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
            try
            {
                while (true)
                {
                    dynamic message;

                    if (_packetBuffer.IsDisposed())
                        return;

                    message = GetProtocol().GetDecoder().Decode(this, _packetBuffer);

                    if (message == null)
                    {
                        BeginReceive();
                        return;
                    }

                    ThreadPool.QueueUserWorkItem((o) =>
                        {
                            try
                            {
                                GetHandler().MessageReceive(this, message);
                            }
                            catch (Exception e)
                            {
                                Log.Error("MessageReceive Error", e);
                            }
                            
                        });
                }
            }
            catch (Exception e)
            {
                Log.Error("Decode Error", e);
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
            {
                try
                {
                    GetHandler().Connected(this);
                }
                catch (Exception e)
                {
                    Log.Error("Connected Error", e);
                }
            }

            BeginReceive();
        }

        protected virtual void BeginReceive()
        {
            
        }

        public virtual void Disconnect()
        {
            if (GetHandler() != null)
            {
                try
                {
                    GetHandler().Disconnected(this);
                }
                catch (Exception e)
                {
                    Log.Error("Disconnected Error", e);
                }
            }
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
