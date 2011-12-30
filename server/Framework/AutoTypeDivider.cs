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
            private Serivce serivce;
            private Job job;

            public DividerEventArgs(Serivce serivce, Job job)
            {
                this.serivce = serivce;
                this.job = job;
            }

            public Serivce getSerivce()
            {
                return this.serivce;
            }

            public Job getJob()
            {
                return this.job;
            }
        }

        private event EventHandler<DividerEventArgs> dividErevent;

        public delegate bool Divider(DividerEventArgs e);
        public delegate void Processor(DividerEventArgs e);

        public void processingJob(Serivce serivce, Job job)
        {
            this.dividErevent(this, new DividerEventArgs(serivce, job));
        }

        public void addProcessor(Divider divider, Processor processor)
        {
            dividErevent += new EventHandler<DividerEventArgs>(new DividerEvent(divider, processor).processingJob);
        }
    }
}
