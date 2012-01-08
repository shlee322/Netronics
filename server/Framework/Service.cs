namespace Netronics
{
    public interface Service
    {
        string GetServiceName();

        void Init();
        void Start();
        void Stop();

        bool IsGroup(string name);
        string[] GetGroupArray();
        double GetLoad();

        bool GetRunning();

        void ProcessingJob(Service service, Job job);
    }
}