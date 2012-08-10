using System.Collections.Generic;
using System.Threading;
using Netronics.Channel.Channel;
using Newtonsoft.Json.Linq;

namespace Service.Manager
{
    class Services
    {
        private readonly string _name;
        private readonly ReaderWriterLockSlim _servicesLock = new ReaderWriterLockSlim();
        private Service[] _services = new Service[100];
        private int _maxIndex = -1;

        public Services(string name)
        {
            _name = name;
        }

        public string GetServicesName()
        {
            return _name;
        }

        public Service JoinService(IChannel channel, int id, JArray address, int port)
        {
            if (id == -1)
            {
                //_servicesLock.EnterWriteLock();

                id = ++_maxIndex;
                if (_maxIndex >= _services.Length) //공간이 작다. 공간을 늘린다.
                {
                    var temp = new Service[_services.Length];
                    System.Array.Copy(_services, temp, _services.Length);
                    _services = temp;
                }
                
                _services[id] = new Service(this, id, address, port);

                dynamic packet = new JObject();
                packet.type = "change_service_id";
                packet.id = id;
                channel.SendMessage(packet);
                //_servicesLock.ExitWriteLock();
            }

            //_servicesLock.EnterReadLock();
            var service = _services[id];
            //_servicesLock.ExitReadLock();

            service.AddChannel(channel);

            return service;
        }

        public dynamic GetServicesNetworkInfo(Network network)
        {
            dynamic packet = new JObject();
            packet.type = "network_info";
            packet.service = GetServicesName();
            packet.network = new JArray();
            //_servicesLock.EnterReadLock();
            for (int i = 0; i <= _maxIndex; i++)
            {
                if(_services[i] == null)
                    continue;
                byte[] host = network.GetAddress(_services[i]);
                if(host != null)
                    packet.network.Add(host);
            }
            //_servicesLock.ExitReadLock();
            return packet;
        }

        public int NotifyJoinService(Service service, Network network)
        {
            int count = 0;
            //이제 연결된 서비스들에게 상태를 보고함.
            for (int i = 0; i <= _maxIndex; i++)
            {
                if(network.GetAddress(_services[i]) == null)
                    continue;
                _services[i].NotifyJoinService(service, network);
                count++;
            }
            return count;
        }
    }
}
