using Netronics.Channel.Channel;
using Netronics.Protocol.PacketEncoder.Http;

namespace Netronics.Http.Handler
{
    interface IUriHandler
    {
        string GetUri();
        bool IsMatch(Request request);
        void Handle(IChannel channel, Request request);
    }
}
