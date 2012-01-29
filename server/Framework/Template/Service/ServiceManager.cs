namespace Netronics.Template.Service
{
    class ServiceManager
    {
        public void ProcessingTask(Service service, Task task)
        {
            new Service().ProcessingTask(service, task);
        }
    }
}
