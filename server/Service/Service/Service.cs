using System.Collections.Generic;

namespace Service.Service
{
    public class Service
    {
        public static LocalService GetService()
        {
            return Entity.GetEntity().GetLocalService();
        }

        public virtual Entity NewEntity()
        {
            if (Entity.GetEntity() == null)
                return null;

            var localService = Entity.GetEntity().GetLocalService();
            if (localService == this)
                return null;

            var entity = localService.NewEntity();
            //entity.AddReceiver(this);
            return entity;
        }

        public Service GetService(string name)
        {
            return null;
        }
        public List<Netronics.Template.Service.Service.Service> GetServices(string name)
        {
            //test
            return null;// new RemoteService();
        }
    }
}
