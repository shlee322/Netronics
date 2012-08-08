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
        private readonly Service[] _services = new Service[100];
        private int _maxIndex = -1;
        private readonly Queue<int> _serviceQueue = new Queue<int>();

        public Services(string name)
        {
            _name = name;
        }

        public string GetServicesName()
        {
            return _name;
        }

        public Service JoinService(IChannel channel, int id, JArray address)
        {
            if (id == -1)
            {
                _servicesLock.EnterWriteLock();
                if (_serviceQueue.Count != 0)
                {
                    id = _serviceQueue.Dequeue();
                }
                else
                {
                    id = ++_maxIndex;
                    if (_maxIndex >= _services.Length) //공간이 작다. 공간을 늘린다.
                    {
                    }
                }
                _services[id] = new Service(this, id, address);
                _servicesLock.ExitWriteLock();
            }

            _servicesLock.EnterReadLock();
            var service = _services[id];
            _servicesLock.ExitReadLock();

            service.AddChannel(channel);

            return service;
        }

        public dynamic GetServicesNetworkInfo(Network network)
        {
            dynamic packet = new JObject();
            packet.type = "network_info";
            packet.service = GetServicesName();
            packet.network = new JArray();
            _servicesLock.EnterReadLock();
            for (int i = 0; i < _services.Length; i++)
            {
                if(_services[i] == null)
                    continue;
                packet.network.Add(network.GetAddress(_services[i]));
            }
            _servicesLock.ExitReadLock();
            return packet;
        }
    }
}
