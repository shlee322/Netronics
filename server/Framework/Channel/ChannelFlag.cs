using System;

namespace Netronics
{
	public class ChannelFlag
	{
		public enum Flag{Encoder, Decoder, Encryptor, Decryptor, Handler}
		
		private Object[] _flag = new Object[5];

        public Object this[Flag index]
		{
			get
			{
				return _flag[(int)index];
			}
			set
			{
				_flag[(int)index] = value;
			}
		}
	}
}

