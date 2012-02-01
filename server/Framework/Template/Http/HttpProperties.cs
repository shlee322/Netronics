using System;
using System.Net;
using Netronics.Channel;

namespace Netronics.Template.HTTP
{
    public class HttpProperties : Properties
    {
        private Func<IChannelHandler> _handler;

        public HttpProperties()
        {
            IpEndPoint = new IPEndPoint(IPAddress.Any, 80);
            ChannelFactory = new HttpChannelFactory(this);
            SetCreateHandler(() =>
                                 {
                                     return new BasicChannelHandler();
                                 });
        }

        public void SetCreateHandler(Func<IChannelHandler> handler)
        {
            _handler = handler;
        }

        public IChannelHandler CreateHandler()
        {
            return _handler();
        }
    }
}
