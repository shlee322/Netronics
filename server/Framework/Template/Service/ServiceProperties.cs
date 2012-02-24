namespace Netronics.Template.Service
{
    internal class ServiceProperties : Properties
    {
        public ServiceProperties(Service.LocalService service)
        {
            var manager = new ServiceManager(service);
            service.SetServiceManager(manager);
            ChannelFactory = new ServiceChannelFactory(manager);
            StartEvent += (sender, args) => service.Start();
            StopEvent += (sender, args) => service.Stop();
        }
    }
}