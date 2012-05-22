
using Netronics.Template.Service.Service;

namespace Framework.Service
{
    class LoginServer : ILoginServer, IRole
    {
        public void Login(string id, string pw)
        {
            var e = Netronics.Template.Service.Entity.GetEntity();
        }
    }
}
