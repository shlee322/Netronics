using System.Collections.Generic;
using System.Threading;
using Netronics.Channel.Channel;
using Newtonsoft.Json.Linq;

namespace Service.Manager
{
    class Services
    {
        private readonly ReaderWriterLockSlim _servicesLock = new ReaderWriterLockSlim();
        private readonly Service[] _services = new Service[100];
        private int _maxIndex = -1;
        private readonly Queue<int> _serviceQueue = new Queue<int>();

        public void JoinService(IChannel channel, int id, JArray address)
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
                _services[id] = new Service(id, address);
                _servicesLock.ExitWriteLock();
            }

            _servicesLock.EnterReadLock();
            var service = _services[id];
            _servicesLock.ExitReadLock();

            service.AddChannel(channel);
        }
    }
}
