using Netronics.Channel;
using Netronics.Channel.Channel;
using Netronics.Template.Service.Protocol;

namespace Netronics.Template.Service.Service
{
    internal sealed class RemoteService : Service
    {
        private IChannel _channel;

        public RemoteService(ServiceManager manager)
        {
            SetServiceManager(manager);
        }

        public override void ProcessingTask(Task.Task task)
        {
            //해당 서비스로 task를 전송
        }
    }
}