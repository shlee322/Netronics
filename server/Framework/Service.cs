﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Netronics
{
    public interface Service
    {
        string getServiceName();

        void init();
        void start();
        void stop();

        bool isGroup(string name);
        string[] getGroupArray();
        double getLoad();

        bool getRunning();

        void processingJob(Service Service, Job job);
    }
}