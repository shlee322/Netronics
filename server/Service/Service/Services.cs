namespace Service.Service
{
    class Services
    {
        private string _name;
        private Service[] _services = new Service[100];

        public Services(string name)
        {
            _name = name;
        }

        public void NotifyJoinService(int id, byte[] network)
        {
            Service service = GetSerivce(id);
            if(service == null)
            {
                if (id >= _services.Length)
                {
                    var temp = new Service[_services.Length];
                    System.Array.Copy(_services, temp, _services.Length);
                    _services = temp;
                }
                _services[id] = new RemoteService(this, id);
            }
            service = GetSerivce(id);
            if(service is RemoteService)
                ((RemoteService)service).AddNetwork(network);
        }

        private Service GetSerivce(int id)
        {
            if (id >= _services.Length || id < 0)
                return null;
            return _services[id];
        }
    }
}
