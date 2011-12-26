using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Netronics;

namespace Test
{
    [TestClass]
    public class JobTest
    {
        [TestMethod]
        public void JobTest1()
        {
            Job job = new Job("map");
            job.group = "all";
            job.message.test = 1;
            job.message.test2 = "abcd";
            job.success += new Job.Result(job_success);
            job.fail += new Job.Result(job_fail);

            job.callSuccess();
            job.callFail();
        }

        void job_fail(Job job)
        {
        }

        void job_success(Job job)
        {
        }
    }
}
