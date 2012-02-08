using System;

namespace Netronics
{
	public class ChannelFlag
	{
		enum Flag{Encoder, Decoder, Encryptor, Decryptor, Handler}
		
		private Object[] _flag = new Object[sizeof(Type)];
		
		public Object this[int index]
		{
			get
			{
				return _flag[index];
			}
			set
			{
				_flag[index] = value;
			}
		}
	}
}

