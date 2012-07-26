using System.Collections.Generic;
using System.Threading;
using Netronics.Channel.Channel;
using Newtonsoft.Json.Linq;

namespace Service.Manager
{
    class Service
    {
        private int _id;
        private byte[][] _address;

        private readonly ReaderWriterLockSlim _channelsLock = new ReaderWriterLockSlim();
        private List<IChannel> _channels = new List<IChannel>(); 

        public Service(int id, IEnumerable<JToken> address)
        {
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
    }
}
