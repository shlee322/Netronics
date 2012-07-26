using System.Collections.Generic;

namespace Service.Coroutine
{
    class Coroutine
    {
        public delegate IEnumerator<IYield> CoroutineEntrance();

        public Coroutine(CoroutineEntrance entrance)
        {
            Co.Current = this;
            y = Co.Call(entrance());
            Co.Current = null;
        }

        public bool Resume()
        {
            Co.Current = this;
            bool ret = y.Resume();
            Co.Current = null;
            return ret;
        }

        private IYield y;
    }
}
