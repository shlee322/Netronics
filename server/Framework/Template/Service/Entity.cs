using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Threading;
using Netronics.Template.Service.Service;

namespace Netronics.Template.Service
{
    public class Entity
    {
        private LocalService _service;
        private IRole _role;
        private static readonly ThreadLocal<Stack<Entity>> Stack = new ThreadLocal<Stack<Entity>>(() => new Stack<Entity>());

        public static Entity CreateEntity(LocalService service, IRole role)
        {
            return new Entity(service) {_role = role};
        }

        public Entity(LocalService service)
        {
            _service = service;
        }

        public void AddReceiver(Service.Service service)
        {
        }

        public void SetGroup(Group group)
        {
        }

        public Group GetGroup()
        {
            return null;
        }

        public static Entity GetEntity()
        {
            return Stack.Value.Peek();
        }

        public object Call<T>(Func<T, object> func)
        {
            Stack.Value.Push(this);
            object o = func((T)_role);
            Stack.Value.Pop();
            return o;
        }

        public void Call<T>(Action<T> func)
        {
            Call<T>(o =>
                        {
                            func(o);
                            return null;
                        });
        }
    }
}
