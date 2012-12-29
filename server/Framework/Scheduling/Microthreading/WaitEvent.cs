using System.Collections.Generic;

namespace Netronics.Scheduling.Microthreading
{
    public class WaitEvent
    {
        private readonly bool _isLock;
        private bool _set = false;
        private readonly List<Microthread> _microthreads = new List<Microthread>();

        public WaitEvent(bool isLock = false)
        {
            _isLock = isLock;
        }

        public void Set()
        {
            if(_isLock)
            {
                lock (_microthreads)
                {
                    SetA();
                    return;
                }
            }
            
            SetA();
        }

        private void SetA()
        {
            _set = true;
            foreach (var microthread in _microthreads)
            {
                Microthread.Run(microthread);
            }
            _microthreads.Clear();
        }

        /// <summary>
        /// 프레임워크 내부적으로 사용되는 메소드입니다.
        /// </summary>
        /// <param name="microthread"></param>
        public void AddWaitMicrothread(Microthread microthread)
        {
            if (_isLock)
            {
                lock (_microthreads)
                {
                    _microthreads.Add(microthread);
                    return;
                }
            }

            _microthreads.Add(microthread);
        }

        public bool IsSet()
        {
            return _set;
        }
    }
}
