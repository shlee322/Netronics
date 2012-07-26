using System.Collections.Generic;

namespace Service.Coroutine
{
    class NestedYield : IYield
    {
        public NestedYield(IEnumerator<IYield> e)
        {
            _e = e;
            _valid = e.MoveNext();
        }

        public bool Resume()
        {
            if (!_valid)
            {
                return false;
            }
            if (_e.Current.Resume())
            {
                return true;
            }
            _valid = _e.MoveNext();
            return _valid;
        }

        private readonly IEnumerator<IYield> _e;
        private bool _valid;

    }
}
