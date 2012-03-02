namespace Netronics.Template.Service
{
    public class ServiceProperties : Properties
    {
        public ServiceProperties(Service.LocalService service)
        {
            var manager = new ServiceManager(service);
            ChannelFactory = new ServiceChannelFactory(manager);
            StartEvent += (sender, args) => service.Start();
            StopEvent += (sender, args) => service.Stop();
        }
    }
}