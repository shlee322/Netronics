﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;

namespace ProxyService
{
    public partial class ProxyService : ServiceBase
    {
        public ProxyService()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            Netronics.Netronics.Service = new Service();
            Netronics.Netronics.Start();
        }

        protected override void OnStop()
        {
            Netronics.Netronics.Stop();
        }
    }
}
