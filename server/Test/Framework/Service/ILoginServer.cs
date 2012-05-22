using Netronics.Template.Service;

namespace Framework.Service
{
    interface ILoginServer
    {
        [Task]
        void Login(string id, string pw);
    }
}
