using Netronics.Protocol;
using Netronics.Protocol.PacketEncoder;
using Netronics.Protocol.PacketEncryptor;

namespace Netronics.Template.Service.Protocol
{
    class Protocol : IProtocol
    {
        private readonly PacketEncoder _encoder;

        public Protocol(ServiceManager manager)
        {
            _encoder = new PacketEncoder(manager);
        }

        public IPacketEncryptor GetEncryptor()
        {
            return null;
        }

        public IPacketDecryptor GetDecryptor()
        {
            return null;
        }

        public IPacketEncoder GetEncoder()
        {
            return _encoder;
        }

        public IPacketDecoder GetDecoder()
        {
            return _encoder;
        }
    }
}
