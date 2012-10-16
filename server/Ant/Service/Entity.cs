using System;
using System.Collections.Generic;
using System.Threading;

namespace Service.Service
{
    public class Entity
    {
        private static readonly ThreadLocal<Stack<Entity>> Stack = new ThreadLocal<Stack<Entity>>(() => new Stack<Entity>());

        private readonly LocalService _service;
        private readonly string _uid;
        
        public static Entity CreateEntity(LocalService service, string uid)
        {
            return new Entity(service, uid);
        }

        public Entity(LocalService service, string uid)
        {
            _service = service;
            _uid = uid;
        }

        public LocalService GetLocalService()
        {
            return _service;
        }

        public static Entity GetEntity()
        {
            try
            {
                return Stack.Value.Peek();
            }
            catch (InvalidOperationException)
            {
                return null;
            }
        }
    }
}
