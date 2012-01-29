namespace Netronics.Template.Service
{
    class ServiceProperties : Properties
    {
        public ServiceProperties(Service service)
        {
            var manager = new ServiceManager();
            service.SetServiceManager(manager);
            ChannelFactory = new ServiceChannelFactory(manager);
            StartEvent += (sender, args) => service.Start();
            StopEvent += (sender, args) => service.Stop();
        }
    }
}
