namespace Netronics.Template.Service
{
    internal class ServiceManager
    {
        public void ProcessingTask(Task task)
        {
            new Service().ProcessingTask(task);
        }
    }
}