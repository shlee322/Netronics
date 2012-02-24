using Netronics.Channel;

namespace Netronics.Template.Service.Service
{
    internal sealed class RemoteService : Service, IChannelHandler
    {
        private IChannel _channel;

        public RemoteService(ServiceManager manager)
        {
            SetServiceManager(manager);
        }

        #region IChannelHandler Members

        public void Connected(IChannel channel)
        {
            _channel = channel;
        }

        public void Disconnected(IChannel channel)
        {
        }

        public void MessageReceive(IChannel channel, dynamic message)
        {
        }

        #endregion

        public override void ProcessingTask(Task.Task task)
        {
            //해당 서비스로 task를 전송
        }
    }
}