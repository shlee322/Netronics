namespace Netronics.Template.Service.Service
{
    public class Service
    {
        protected Service()
        {
        }

        public static Service GetService()
        {
            return Entity.GetEntity().GetLocalService();
        }

        public virtual Entity NewEntity()
        {
            if(Entity.GetEntity() == null)
                return null;

            var localService = Entity.GetEntity().GetLocalService();
            if (localService == this)
                return null;

            var entity = localService.NewEntity();
            entity.AddReceiver(this);
            return entity;
        }

        public Service GetService(string name)
        {
            //test
            return new RemoteService();
        }
    }
}
