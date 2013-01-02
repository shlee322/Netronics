using System.Collections.Generic;
using Netronics.Channel.Channel;
using Newtonsoft.Json.Linq;

namespace Netronics.Ant.QueenAnt
{
    class Ant
    {
        private Ants _ants;
        private int _id;
        private int _port;

        private IChannel _channel;

        private IList<byte[]> _addressList; 

        public Ant(Ants ants, int id, int port)
        {
            _ants = ants;
            _id = id;
            _port = port;
        }

        public void AddChannel(IChannel channel)
        {
            _channel = channel;
            channel.SetTag(this);
        }

        public void AddAddress(IEnumerable<byte[]> address)
        {
            _addressList = new List<byte[]>(address);
        }

        public Ants GetAnts()
        {
            return _ants;
        }

        public IEnumerable<byte[]> GetAddress()
        {
            return _addressList;
        }

        public int GetId()
        {
            return _id;
        }

        public int GetPort()
        {
            return _port;
        }
    }
}
