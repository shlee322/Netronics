using Netronics.Channel.Channel;
using Service.Service.Manager;
using Service.Service.Task;

namespace Service.Service
{
    public class Services
    {
        private string _name;
        private ManagerProcessor _managerProcessor;
        private Service[] _services = new Service[100];
        private int _maxService=0;

        public Services(ManagerProcessor managerProcessor,string name)
        {
            _managerProcessor = managerProcessor;
            _name = name;
        }

        public void NotifyJoinService(int id, byte[] address, int port)
        {
            Service service = GetSerivce(id, true);
            if(service is RemoteService)
                ((RemoteService)service).AddNetwork(address, port);
        }

        public ManagerProcessor GetManagerProcessor()
        {
            return _managerProcessor;
        }

        private Service GetSerivce(int id, bool create = false)
        {
            if(!create)
            {
                if (id >= _services.Length || id < 0)
                    return null;
                return _services[id];
            }
            Service service = GetSerivce(id);
            if(service == null)
            {
                if (id >= _maxService)
                    _maxService = id + 1;

                if (id >= _services.Length)
                {
                    var temp = new Service[id+10];
                    System.Array.Copy(_services, temp, _services.Length);
                    _services = temp;
                }
                _services[id] = new RemoteService(this, id);
                service = _services[id];
            }
            return service;
        }

        public void ConnectServiceInfo(int id, IChannel channel)
        {
            Service service = GetSerivce(id, true);
            if (service is RemoteService)
                ((RemoteService)service).AddChannel(channel);
        }

        public void SendTask(Request request)
        {
            if (request.Service == null)
                request.Service = GetTarget(request.Uid);
            request.Service.SendTask(request);
        }

        private Service GetTarget(long uid)
        {
            var services = _services;
            var service = services[uid % services.Length];
            return service;
        }
    }
}
