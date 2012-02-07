using System;
using System.Net.Sockets;
using Netronics.PacketEncoder;
using Netronics.PacketEncoder.Bson;

namespace Netronics.Channel
{
    public class BasicChannelFactory : IChannelFactory
    {
        private Func<IPacketEncoder> _encoder = () => new BsonEncoder();
        private Func<IPacketDecoder> _decoder = () => new BsonDecoder();
		private Func<IPacketEncryptor> _encryptor;
		private Func<IPacketDecryptor> _decryptor;
        private Func<IChannelHandler> _handler = () => new BasicChannelHandler(); 

        public Channel CreateChannel(Netronics netronics, Socket socket)
        {
            return Channel.CreateChannel(socket,
			                             _encoder(),
			                             _decoder(),
			                             _encryptor != null ? _encryptor() : null,
			                             _decryptor != null ? _decryptor() : null,
			                             _handler());
        }

        public BasicChannelFactory SetPacketEncoder(Func<IPacketEncoder> func)
        {
            _encoder = func;
            return this;
        }

        public BasicChannelFactory SetPacketDecoder(Func<IPacketDecoder> func)
        {
            _decoder = func;
            return this;
        }
		
		public BasicChannelFactory SetPacketEncryptor(Func<IPacketEncryptor> func)
		{
			_encryptor = func;
		}
		
		public BasicChannelFactory SetPacketDecryptor(Func<IPacketDecryptor> func)
		{
			_decryptor = func;
		}
		
		

        public BasicChannelFactory SetHandler(Func<IChannelHandler> func)
        {
            _handler = func;
            return this;
        }
    }
}
