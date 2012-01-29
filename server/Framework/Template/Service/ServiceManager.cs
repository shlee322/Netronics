namespace Netronics.Template.Service
{
    class ServiceManager
    {
        public void ProcessingTask(Task task)
        {
            new Service().ProcessingTask(task);
        }
    }
}
