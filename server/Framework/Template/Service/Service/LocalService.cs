using System;

namespace Netronics.Template.Service.Service
{
    public class LocalService : Service
    {
        private Type _roleType;
        private Func<IRole> _createRoleFunc;

        public LocalService(string managerHost, int port=1005)
        {
        }

        public void AddRole<T>(Func<IRole> createRoleFunc)
        {
            _roleType = typeof (T);
            _createRoleFunc = createRoleFunc;
        }

        public override Entity NewEntity()
        {
            var entity = base.NewEntity();
            if (entity != null)
                return entity;

            var role = _createRoleFunc();

            if (!_roleType.IsInstanceOfType(role))
            {
                throw new Exception("Role와 Type가 일치하지 않습니다.");
            }

            return Entity.CreateEntity(this, role);
        }
    }
}
