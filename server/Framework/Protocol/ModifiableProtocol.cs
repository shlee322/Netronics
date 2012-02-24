using Netronics.Protocol.HandShake;
using Netronics.Protocol.PacketEncoder;
using Netronics.Protocol.PacketEncryptor;

namespace Netronics.Protocol
{
    public class ModifiableProtocol : IProtocol
    {
        private IPacketEncryptor _encryptor;
        private IPacketDecryptor _decryptor;
        private IPacketEncoder _encoder;
        private IPacketDecoder _decoder;

        public ModifiableProtocol(IPacketEncryptor encryptor=null, IPacketDecryptor decryptor=null, IPacketEncoder encoder=null, IPacketDecoder decoder=null)
        {
            _encryptor = encryptor;
            _decryptor = decryptor;
            _encoder = encoder;
            _decoder = decoder;
        }

        public IPacketEncryptor GetEncryptor()
        {
            return _encryptor;
        }

        public IPacketDecryptor GetDecryptor()
        {
            return _decryptor;
        }

        public IPacketEncoder GetEncoder()
        {
            return _encoder;
        }

        public IPacketDecoder GetDecoder()
        {
            return _decoder;
        }
    }
}
