using Netronics.Protocol;
using Netronics.Protocol.PacketEncoder;
using Netronics.Protocol.PacketEncryptor;

namespace Netronics.Template.Service.Protocol
{
    class Protocol : IProtocol
    {
        private static readonly Protocol Instance = new Protocol();
        private static readonly PacketEncoder Encoder = new PacketEncoder();

        public static Protocol GetInstance()
        {
            return Instance;
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
            return Encoder;
        }

        public IPacketDecoder GetDecoder()
        {
            return Encoder;
        }
    }
}
