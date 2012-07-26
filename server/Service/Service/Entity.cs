using System;
using System.Collections.Generic;
using System.Threading;
using Service.Coroutine;

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

        public IYield Call<T>(Func<T, IEnumerator<IYield>> func) where T : class
        {
            var t = _service as T;
            if(t == null)
                throw new Exception("등록된 서비스가 Call 대상을 상속하고 있지 않습니다.");
            Stack.Value.Push(this);
            var o = new NestedYield(func(t)); //내부에서 패킷 전송후 yield return new NonNestedYield();
            Stack.Value.Pop();
            return o;
        }

        public IEnumerator<IYield> Call<T>(Action<T> func)
        {
            return null;/*
            Call<T>(o =>
                        {
                            func(o);
                            return null;
                        });*/
        }
    }
}
