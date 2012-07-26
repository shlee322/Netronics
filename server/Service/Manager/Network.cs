namespace Service.Manager
{
    class Network
    {
        public string Service1;
        public string Service2;
        public byte[] Subnet;
        public byte[] Mask;

        public Network(string service1, string service2, byte[] subnet, byte[] mask)
        {
            Service1 = service1;
            Service2 = service2;
            Subnet = subnet;
            Mask = mask;
        }
    }
}
