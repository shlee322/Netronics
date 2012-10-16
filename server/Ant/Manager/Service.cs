using System.Collections.Generic;
using System.Threading;
using Netronics.Channel.Channel;
using Netronics.Template.Ant.QueenAnt;
using Newtonsoft.Json.Linq;

namespace Service.Manager
{
    class Service
    {
        private readonly Services _services;
        private int _id;
        private byte[][] _address;
        private int _port;

        private readonly ReaderWriterLockSlim _channelsLock = new ReaderWriterLockSlim();
        private List<IChannel> _channels = new List<IChannel>(); 

        public Service(Services services, int id, IEnumerable<JToken> address, int port)
        {
            _services = services;
            _id = id;
            var addressList = new List<byte[]>();
            foreach (var bytes in address)
                addressList.Add((byte[]) bytes);
            _address = addressList.ToArray();
            _port = port;
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

        public void NotifyJoinService(Service service, Network network)
        {
            dynamic packet = new JObject();
            packet.type = "notify_join_service";
            packet.service = service.GetServiceName();
            packet.id = service._id;
            packet.address = network.GetAddress(service);
            packet.port = service._port;
            this.SendPacket(packet);
        }

        private void SendPacket(dynamic packet)
        {
            _channelsLock.EnterReadLock();
            _channels[0].SendMessage(packet);
            _channelsLock.ExitReadLock();
        }
    }
}
