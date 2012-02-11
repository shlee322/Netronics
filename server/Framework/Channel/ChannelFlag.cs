using System;

namespace Netronics
{
	public class ChannelFlag
	{
        public enum Flag { Encoder, Decoder, Encryptor, Decryptor, Handler, Parallel}

	    private Object[] _flag = new Object[]{null, null, null, null, null, false};

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

