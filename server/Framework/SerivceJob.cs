﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Netronics
{
    public class SerivceJob : Job
    {
        static public SerivceJob joinGroup(string name)
        {
            SerivceJob job = new SerivceJob();
            job.message.type = "iolnGroup";
            job.message.name = name;
            return job;
        }

        static public SerivceJob dropGroup(string name)
        {
            SerivceJob job = new SerivceJob();
            job.message.type = "dropGroup";
            job.message.name = name;
            return job;
        }

        private SerivceJob()
            : base("Netronics")
        {
        }
    }
}
