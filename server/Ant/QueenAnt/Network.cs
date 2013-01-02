using System;
using System.Collections.Generic;

namespace Netronics.Ant.QueenAnt
{
    class Network
    {
        public string Service1;
        public string Service2;
        public int Service1Id;
        public int Service2Id;
        public byte[] Subnet;
        public byte[] Mask;

        public Network(string service1, string service2, byte[] subnet, byte[] mask)
        {
            Service1 = service1;
            Service2 = service2;
            Subnet = subnet;
            Mask = mask;
            if(Subnet.Length != Mask.Length)
                throw new Exception("network.ns의 Subnet와 Mask의 주소가 잘못되었습니다.");
        }

        private bool ByteArrayCompare(byte[] array1, byte[] array2)
        {
            if (array1.Length != array2.Length)
                return false;
            for (int i = 0; i < array1.Length; i++)
            {
                if (array1[i] != array2[i])
                    return false;
            }
            return true;
        }
        
        public bool Check(Ant ant)
        {
            if (Service1 != ant.GetAnts().GetName() && Service2 != ant.GetAnts().GetName())
                return false;

            foreach (var ip in ant.GetAddress())
            {
                if (Check(ip))
                    return true;
            }
            return false;
        }

        public bool Check(byte[] address)
        {
            if (address.Length != Mask.Length)
                return false;
            var serivceSubnet = new byte[Mask.Length];
            for (int i = 0; i < Mask.Length; i++)
                serivceSubnet[i] = (byte)(address[i] & Mask[i]);
            if (ByteArrayCompare(Subnet, serivceSubnet))
                return true;
            return false;
        }

        public IEnumerable<byte[]> GetAddress(Ant ant)
        {
            if (Service1 != ant.GetAnts().GetName() && Service2 != ant.GetAnts().GetName())
                return null;

            var _list = new List<byte[]>();
            foreach (var address in ant.GetAddress())
            {
                if (Check(address))
                {
                    _list.Add(address);
                }
            }
            return _list;
        }
    }
}
