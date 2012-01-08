using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using Netronics;

namespace Test
{
    [TestFixture()]
    public class JobTest
    {
        [Test()]
        public void JobTest1()
        {
            Job job = new Job("map");
            job.Group = "all";
            job.Take = 3;
            job.Message.test = 1;
            job.Message.test2 = "abcd";
            job.Success += new Job.ResultDelegate(job_success);
            job.Fail += new Job.ResultDelegate(job_fail);

            job.SetReceiver();

            job.ReturnResult(null, true);
        }

        [Test()]
        public void JobTest2()
        {
            Job job = new Job("item");
            job.Group = "all";
            job.Take = 3;
            job.Message.test = 1;
            job.Message.test2 = "abcd";
            job.Success += new Job.ResultDelegate(job_success);
            job.Fail += new Job.ResultDelegate(job_fail);

            job.SetReceiver();
            job.ReturnResult(null, false);
        }

        void job_fail(Service sender, Job.ResultEventArgs e)
        {
        }

        void job_success(Service sender, Job.ResultEventArgs e)
        {
        }
    }
}
