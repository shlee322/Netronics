using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Netronics
{
    public class AutoTypeDivider
    {
        class DividerEvent
        {
            private Divider divider;
            private Processor processor;

            public DividerEvent(Divider divider, Processor processor)
            {
                this.divider = divider;
                this.processor = processor;
            }

            public void processingJob(object sender, DividerEventArgs e)
            {
                if (!this.divider(e))
                    return;
                this.processor(e);
            }
        }

        public class DividerEventArgs : EventArgs
        {
            private Service service;
            private Job job;

            public DividerEventArgs(Service service, Job job)
            {
                this.service = service;
                this.job = job;
            }

            public Service getService()
            {
                return this.service;
            }

            public Job getJob()
            {
                return this.job;
            }
        }

        private event EventHandler<DividerEventArgs> dividErevent;

        public delegate bool Divider(DividerEventArgs e);
        public delegate void Processor(DividerEventArgs e);

        public void processingJob(Service service, Job job)
        {
            this.dividErevent(this, new DividerEventArgs(service, job));
        }

        public void addProcessor(Divider divider, Processor processor)
        {
            dividErevent += new EventHandler<DividerEventArgs>(new DividerEvent(divider, processor).processingJob);
        }
    }
}
