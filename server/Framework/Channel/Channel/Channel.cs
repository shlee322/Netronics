﻿using System;
using System.Collections.Generic;
using Netronics.Protocol.PacketEncoder;
using Netronics.Protocol.PacketEncoder.Linefeed;
using log4net;

namespace Netronics.Channel.Channel
{
    /// <summary>
    /// Netronics에서 사용하는 기본적인 Channel 클래스
    /// </summary>
    public class Channel : IChannel
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(Channel)); 

        private readonly PacketBuffer _packetBuffer = new PacketBuffer();

        private readonly Dictionary<string, object> _config = new Dictionary<string, object>();

        public Channel()
        {
            SetConfig("buffer_size", 0);
            //SetConfig("timeout", 0);
            SetConfig("encoder", LinefeedEncoder.Encoder);
            SetConfig("decoder", LinefeedEncoder.Encoder);
            SetConfig("switch", DefaultReceiveSwitch.Switch);
            SetConfig("scheduler", Scheduling.Scheduler.Default);
        }

        public void SetConfig(string name, object value)
        {
            _config[name] = value;
        }

        public object GetConfig(string name)
        {
            try
            {
                return _config[name];
            }
            catch (KeyNotFoundException)
            {
            }
            return null;
        }

        protected Scheduling.Scheduler GetScheduler()
        {
            return (Scheduling.Scheduler)GetConfig("scheduler");
        }

        protected bool ReceivePacket(byte[] buffer, int len)
        {
            if (_packetBuffer.IsDisposed())
                return false;
            GetScheduler().QueueWorkItem(GetHashCode(), () =>
                {
                    _packetBuffer.Write(buffer, 0, len);
                    _packetBuffer.BeginBufferIndex();

                    var size = (int) GetConfig("buffer_size");
                    if(size > 0 && _packetBuffer.AvailableBytes() > size)
                    {
                        Disconnect();
                        return;
                    }
                    Receive();
                });

            return true;
        }

        protected virtual void Disconnected()
        {
           _packetBuffer.Dispose();
        }

        private void Receive()
        {
            try
            {
                object message;

                if (_packetBuffer.IsDisposed())
                    return;

                try
                {
                    _packetBuffer.BeginBufferIndex();
                    if(_packetBuffer.AvailableBytes() == 0)
                        throw new PacketLengthException();
                    message = ((IPacketDecoder)GetConfig("decoder")).Decode(this, _packetBuffer);
                    _packetBuffer.EndBufferIndex();
                }
                catch (PacketLengthException)
                {
                    _packetBuffer.ResetBufferIndex();
                    BeginReceive();
                    return;
                }
                
                if (message == null)
                {
                    BeginReceive();
                    return;
                }

                var context = new MessageContext(this, message);
                GetScheduler().QueueWorkItem(((IReceiveSwitch)GetConfig("switch")).ReceiveSwitching(context), () =>
                {
                            try
                            {
                                ((IChannelHandler)GetConfig("handler")).MessageReceive(context);
                            }
                            catch (Exception e)
                            {
                                Log.Error("MessageReceive Error", e);
                            }
                            
                        });

                GetScheduler().QueueWorkItem(GetHashCode(), Receive);
            }
            catch (Exception e)
            {
                Log.Error("Decode Error", e);
            }
        }

        public void Connect()
        {
            GetScheduler().QueueWorkItem(GetHashCode(), Connecting);
        }

        public virtual void Connecting()
        {
            var context = new ConnectContext(this);
            GetScheduler().QueueWorkItem(((IReceiveSwitch)GetConfig("switch")).ReceiveSwitching(context), () =>
            {
                try
                {
                    ((IChannelHandler)GetConfig("handler")).Connected(context);
                }
                catch (Exception e)
                {
                    Log.Error("Connected Error", e);
                }
                BeginReceive();
            });

        }

        protected virtual void BeginReceive()
        {
        }

        public void Disconnect()
        {
            var context = new DisconnectContext(this);
            try
            {
                GetScheduler().QueueWorkItem(GetHashCode(), () =>
                        {
                            if (_packetBuffer.IsDisposed())
                                return;
                            Disconnecting();
                            GetScheduler().QueueWorkItem(((IReceiveSwitch)GetConfig("switch")).ReceiveSwitching(context), () =>
                                {
                                    ((IChannelHandler) GetConfig("handler")).Disconnected(context);
                                    GetScheduler().QueueWorkItem(GetHashCode(), () =>
                                    {
                                        if (_packetBuffer.IsDisposed())
                                            return;
                                        Disconnected();
                                    });
                                });
                        });
            }
            catch (Exception e)
            {
                Log.Error("Disconnected Error", e);
            }
        }

        protected virtual void Disconnecting()
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
