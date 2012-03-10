namespace Netronics.Template.Service
{
    public class ServiceProperties : Properties
    {
        public ServiceProperties(Service.LocalService service)
        {
            var manager = new ServiceManager(service);
            ChannelFactory = new ServiceChannelFactory(manager);
            StartEvent += (sender, args) => manager.Start((Netronics)sender);
            StopEvent += (sender, args) => manager.Stop();
        }
    }
}