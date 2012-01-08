using System;

namespace Netronics
{
    public class AutoTypeDivider
    {
        #region Delegates

        public delegate bool Divider(DividerEventArgs e);

        public delegate void Processor(DividerEventArgs e);

        #endregion

        private event EventHandler<DividerEventArgs> DividErevent;

        public void ProcessingJob(Service service, Job job)
        {
            DividErevent(this, new DividerEventArgs(service, job));
        }

        public void AddProcessor(Divider divider, Processor processor)
        {
            DividErevent += new DividerEvent(divider, processor).ProcessingJob;
        }

        #region Nested type: DividerEvent

        private class DividerEvent
        {
            private readonly Divider _divider;
            private readonly Processor _processor;

            public DividerEvent(Divider divider, Processor processor)
            {
                _divider = divider;
                _processor = processor;
            }

            public void ProcessingJob(object sender, DividerEventArgs e)
            {
                if (!_divider(e))
                    return;
                _processor(e);
            }
        }

        #endregion

        #region Nested type: DividerEventArgs

        public class DividerEventArgs : EventArgs
        {
            private readonly Job _job;
            private readonly Service _service;

            public DividerEventArgs(Service service, Job job)
            {
                _service = service;
                _job = job;
            }

            public Service GetService()
            {
                return _service;
            }

            public Job GetJob()
            {
                return _job;
            }
        }

        #endregion
    }
}