using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using Netronics.Template.Service.Service;

namespace Netronics.Template.Service
{
    class Broadcast
    {
        private ServiceManager _manager;
        private Netronics _netronics;

        private Socket _broadcastSocket;
        private byte[] _broadcastSendBuffer = new byte[6];
        private Timer _broadcastTimer;


        private readonly ReaderWriterLockSlim _localServiceListLock = new ReaderWriterLockSlim();
        private readonly List<byte[]> _localServiceList = new List<byte[]>();

        public Broadcast(ServiceManager manager, Netronics netronics)
        {
            _manager = manager;
            _netronics = netronics;

            _broadcastSocket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
            try
            {
                _broadcastSocket.Bind(new IPEndPoint(IPAddress.Any, 1005));
            }
            catch (Exception e)
            {
                _broadcastSocket.Bind(new IPEndPoint(IPAddress.Any, 0));
            }

            new Thread(ReceiveBroadcast).Start();

            byte[] portData = BitConverter.GetBytes((ushort)_netronics.GetEndIPPoint().Port);
            Array.Reverse(portData);
            portData.CopyTo(_broadcastSendBuffer, 0);
            byte[] idData = BitConverter.GetBytes(manager.GetLocalService().GetID());
            Array.Reverse(idData);
            idData.CopyTo(_broadcastSendBuffer, 2);
            

            _broadcastTimer = new Timer(BroadcastTimerCallback);
            _broadcastTimer.Change(1000, 10000);
        }

        private ServiceManager GetServiceManager()
        {
            return _manager;
        }

        private void BroadcastTimerCallback(object state)
        {
            _broadcastSocket.SendTo(_broadcastSendBuffer, new IPEndPoint(((IPEndPoint)_broadcastSocket.LocalEndPoint).Port == 1005 ? IPAddress.Broadcast : IPAddress.Loopback, 1005));
        }

        private void ReceiveBroadcast()
        {
            var data = new byte[6];
            EndPoint point = new IPEndPoint(IPAddress.Any, 1005);
            while (true)
            {
                int len = _broadcastSocket.ReceiveFrom(data, 0, 6, SocketFlags.None, ref point);
                if (len == 6)
                {
                    byte[] portBytes = new byte[2];
                    Array.Copy(data, 0, portBytes, 0, 2);
                    Array.Reverse(portBytes);
                    byte[] idBytes = new byte[4];
                    Array.Copy(data, 2, idBytes, 0, 4);
                    Array.Reverse(idBytes);

                    var service =
                        GetServiceManager().GetOrAddService(BitConverter.ToInt32(idBytes, 0)) as RemoteService;
                    var serviceIPEndPoint = new IPEndPoint(((IPEndPoint)point).Address, BitConverter.ToUInt16(portBytes, 0));

                    if(!GetServiceManager().ExistServiceAddress(service, ((IPEndPoint)point).Address))
                    {
                        var client = new TcpClient();
                        client.Connect(serviceIPEndPoint);
                        _netronics.AddSocket(client.Client).Connect();
                    }
                    
                    if (IPAddress.IsLoopback(serviceIPEndPoint.Address))
                    {
                        _localServiceListLock.EnterReadLock();
                        if (!_localServiceList.Exists(obj =>
                                                          {
                                                              var pBytes = new byte[2];
                                                              Array.Copy(obj, 0, pBytes, 0, 2);
                                                              Array.Reverse(pBytes);
                                                              return serviceIPEndPoint.Port ==
                                                                     BitConverter.ToUInt16(pBytes, 0);
                                                          }))
                        {
                            _localServiceListLock.ExitReadLock();
                            _localServiceListLock.EnterWriteLock();
                            var d = new byte[6];
                            data.CopyTo(d,0);
                            _localServiceList.Add(d);
                            _localServiceListLock.ExitWriteLock();
                            _localServiceListLock.EnterReadLock();
                        }

                        _localServiceList.ForEach((d) =>
                        {
                            var pBytes = new byte[2];
                            Array.Copy(d, 0, pBytes, 0, 2);
                            Array.Reverse(pBytes);
                            if (serviceIPEndPoint.Port == BitConverter.ToUInt16(pBytes, 0))
                                return;
                            _broadcastSocket.SendTo(d, point);
                        });
                        _localServiceListLock.ExitReadLock();
                    }
                }
            }
        }
    }
}
