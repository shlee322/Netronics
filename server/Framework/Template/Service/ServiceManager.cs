using System;
using System.Collections.Generic;
using System.Threading;
using Netronics.Channel;
using Netronics.Channel.Channel;
using Netronics.Template.Service.Protocol;
using Netronics.Template.Service.Service;

namespace Netronics.Template.Service
{
    public class ServiceManager : IChannelHandler
    {
        public static int TransactionSize = 1000;

        private readonly LocalService _localService;
        private readonly Dictionary<int, Service.Service> _services = new Dictionary<int, Service.Service>();
        private readonly ReaderWriterLockSlim _serviceLock = new ReaderWriterLockSlim();

        public ServiceManager(LocalService localService)
        {
            _localService = localService;
            _localService.SetServiceManager(this);
            _services.Add(_localService.GetID(), localService);
        }

        public void ProcessingTask(Task.Task task)
        {
        }

        public LocalService GetLocalService()
        {
            return _localService;
        }

        public Service.Service GetService(int id)
        {
            _serviceLock.EnterReadLock();

            Service.Service service = null;
            try
            {
                service = _services[id];
            }
            catch (KeyNotFoundException)
            {
                _serviceLock.ExitReadLock();
                return null;
            }
            _serviceLock.ExitReadLock();
            return service;
        }
        
        public Service.Service GetOrAddService(int id)
        {
            Service.Service service = GetService(id);
            if (service != null)
                return service;
            _serviceLock.EnterWriteLock();
            try
            {
                service = _services[id];
                _serviceLock.ExitWriteLock();
                return service;
            }
            catch (KeyNotFoundException)
            {
            }
            service = new RemoteService(this, id);
            _services.Add(id, service);
            _serviceLock.ExitWriteLock();
            return service;
        }

        public Type GetMessageType(string messageName)
        {
            return _localService.GetProcessor(messageName).MessageType;
        }

        public Type GetResultObjectType(string resultObjectType)
        {
            return _localService.GetResultObjectType(resultObjectType);
        }

        public void Connected(IChannel channel)
        {
            channel.SendMessage(new IDInfo { ID = _localService.GetID()});
        }

        public void Disconnected(IChannel channel)
        {
        }

        public void MessageReceive(IChannel channel, dynamic message)
        {
            if (message is Request)
                ProcessingRequest(channel, message);
            else if(message is Result)
                ProcessingResult(channel, message);
            else if(message is IDInfo)
                ProcessingIDInfo(channel, message);
        }

        private void ProcessingResult(IChannel channel, Result result)
        {
            if(result.Receiver == _localService.GetID())
            {
                var service = GetService(result.Sender) as RemoteService;
                if(service != null)
                {
                    Task.Task task = service.GetOrRemoveTransaction((int) result.Transaction);
                    task.Result(result.ResultObject, result.Success);
                }
            }else
            {
            }
        }

        private void ProcessingIDInfo(IChannel channel, IDInfo message)
        {
            Service.Service service = GetOrAddService(message.ID);
            if(service is RemoteService)
            {
                ((RemoteService)service).SetChannel(channel);
            }
        }

        private void ProcessingRequest(IChannel channel, Request request)
        {
            if(request.Receiver == _localService.GetID())
                _localService.GetProcessor(request.Message.GetType().FullName).Action(Task.Task.GetTask(channel, request));
        }
    }
}