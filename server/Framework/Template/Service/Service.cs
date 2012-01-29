namespace Netronics.Template.Service
{
    class Service
    {
        private ServiceManager _manager;

        public virtual void Start()
        {
            GetServiceManager().ProcessingTask(this, null);
        }

        public virtual void Stop()
        {
        }

        public virtual void ProcessingTask(Service service, Task task)
        {
        }

        public virtual void SetServiceManager(ServiceManager manager)
        {
            _manager = manager;
        }

        public virtual ServiceManager GetServiceManager()
        {
            return _manager;
        }
    }
}
