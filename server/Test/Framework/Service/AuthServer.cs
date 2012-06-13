using Netronics.Template.Service.Service;

namespace Framework.Service
{
    class AuthServer : IAuthServer, IRole
    {
        public string GetUserInfo(string id)
        {
            return "userdata";
        }
    }
}
