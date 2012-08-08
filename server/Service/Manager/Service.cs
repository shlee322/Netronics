using System.Collections.Generic;
using System.Threading;
using Netronics.Channel.Channel;
using Newtonsoft.Json.Linq;

namespace Service.Manager
{
    class Service
    {
        private readonly Services _services;
        private int _id;
        private byte[][] _address;

        private readonly ReaderWriterLockSlim _channelsLock = new ReaderWriterLockSlim();
        private List<IChannel> _channels = new List<IChannel>(); 

        public Service(Services services, int id, IEnumerable<JToken> address)
        {
            _services = services;
            _id = id;
            var addressList = new List<byte[]>();
            foreach (var bytes in address)
                addressList.Add((byte[]) bytes);
            _address = addressList.ToArray();
        }

        public void AddChannel(IChannel channel)
        {
            _channelsLock.EnterWriteLock();
            _channels.Add(channel);
            _channelsLock.ExitWriteLock();
        }
        public string GetServiceName()
        {
            return _services.GetServicesName();
        }

        public byte[][] GetAddress()
        {
            return _address;
        }
    }
}
