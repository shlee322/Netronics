﻿using System;
using Netronics.Template.Service.Service;

namespace Netronics.Template.Service
{
    public class ServiceManager
    {
        private LocalService _localService;

        public ServiceManager(LocalService localService)
        {
            _localService = localService;
        }

        public void ProcessingTask(Task.Task task)
        {
        }
    }
}