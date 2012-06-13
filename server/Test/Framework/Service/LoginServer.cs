
using Netronics.Template.Service.Service;

namespace Framework.Service
{
    class LoginServer : ILoginServer, IRole
    {
        public void Login(string id, string pw)
        {
            //var e = Netronics.Template.Service.Entity.GetEntity();
            //NewEntity하면 내부에서 GetEntity해서 NewEntity을 호출한 서비스를 찾는다.
            //이코드의 경우 LocalSerivce가 잡히겠지.
            //어쨋든 그 서비스랑 GetSerivce한 서비스랑 통신!

            //Netronics.Template.Service.Service.Service.GetService().GetService("AuthServer").NewEntity();
        }
    }
}
