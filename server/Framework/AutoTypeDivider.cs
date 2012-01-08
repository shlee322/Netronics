using System;

namespace Netronics
{
    public class AutoTypeDivider
    {
        #region Delegates

        public delegate bool Divider(DividerEventArgs e);

        public delegate void Processor(DividerEventArgs e);

        #endregion

        private event EventHandler<DividerEventArgs> dividErevent;

        public void processingJob(Service service, Job job)
        {
            dividErevent(this, new DividerEventArgs(service, job));
        }

        public void addProcessor(Divider divider, Processor processor)
        {
            dividErevent += new DividerEvent(divider, processor).processingJob;
        }

        #region Nested type: DividerEvent

        private class DividerEvent
        {
            private readonly Divider divider;
            private readonly Processor processor;

            public DividerEvent(Divider divider, Processor processor)
            {
                this.divider = divider;
                this.processor = processor;
            }

            public void processingJob(object sender, DividerEventArgs e)
            {
                if (!divider(e))
                    return;
                processor(e);
            }
        }

        #endregion

        #region Nested type: DividerEventArgs

        public class DividerEventArgs : EventArgs
        {
            private readonly Job job;
            private readonly Service service;

            public DividerEventArgs(Service service, Job job)
            {
                this.service = service;
                this.job = job;
            }

            public Service getService()
            {
                return service;
            }

            public Job getJob()
            {
                return job;
            }
        }

        #endregion
    }
}