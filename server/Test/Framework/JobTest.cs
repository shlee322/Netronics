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
            job.group = "all";
            job.take = 3;
            job.message.test = 1;
            job.message.test2 = "abcd";
            job.success += new Job.Result(job_success);
            job.fail += new Job.Result(job_fail);

            job.setReceiver();

            job.returnResult(null, true);
        }

        [Test()]
        public void JobTest2()
        {
            Job job = new Job("item");
            job.group = "all";
            job.take = 3;
            job.message.test = 1;
            job.message.test2 = "abcd";
            job.success += new Job.Result(job_success);
            job.fail += new Job.Result(job_fail);

            job.setReceiver();
            job.returnResult(null, false);
        }

        void job_fail(Service sender, Job.ResultEventArgs e)
        {
        }

        void job_success(Service sender, Job.ResultEventArgs e)
        {
        }
    }
}
