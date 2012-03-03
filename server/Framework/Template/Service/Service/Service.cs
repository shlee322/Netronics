namespace Netronics.Template.Service.Service
{
    public class Service
    {
        private ServiceManager _manager;

        public virtual void Start()
        {
        }

        public virtual void Stop()
        {
        }

        public virtual void ProcessingTask(Task.Task task)
        {
            //task.Job.ProcessingTask(this, task);
        }

        public virtual void SetServiceManager(ServiceManager manager)
        {
            _manager = manager;
        }

        public virtual ServiceManager GetServiceManager()
        {
            return _manager;
        }

        public virtual int GetID()
        {
            return 0;
        }
    }
}