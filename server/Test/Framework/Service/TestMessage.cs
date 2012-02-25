using Netronics.Template.Service;
using Netronics.Template.Service.Task;

namespace Framework.Service
{
    [Message("testService", 3)]
    class TestMessage
    {
        public string name;
        public int test;
    }
}
