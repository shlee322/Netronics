
using Netronics.Template.Service;

namespace Framework.Service
{
    interface IAuthServer
    {
        [Task]
        string GetUserInfo(string id);
    }
}
