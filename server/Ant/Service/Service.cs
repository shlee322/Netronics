using System.Collections.Generic;
using Service.Service.Task;

namespace Service.Service
{
    public class Service
    {
        private Manager.ManagerProcessor _processor;

        public void SetManagerProcessor(Manager.ManagerProcessor processor)
        {
            _processor = processor;
        }

        public Manager.ManagerProcessor GetManagerProcessor()
        {
            return _processor;
        }

        public static LocalService GetService()
        {
            return null;
            //return Entity.GetEntity().GetLocalService();
        }

        public virtual Entity NewEntity()
        {
            return null;
            /*
            if (Entity.GetEntity() == null)
                return null;

            var localService = Entity.GetEntity().GetLocalService();
            if (localService == this)
                return null;

            var entity = localService.NewEntity();
            //entity.AddReceiver(this);
            return entity;*/
        }

        public Services GetServices(string name)
        {
            return _processor.GetServices(name);
        }

        public void SendTask(Request request)
        {
        }
    }
}
