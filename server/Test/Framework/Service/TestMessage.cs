using Netronics.Template.Service.Task;

namespace Framework.Service
{
    [Message("testService", 3)]
    class TestMessage
    {
        public string Name;
        public long Test;
    }
}
