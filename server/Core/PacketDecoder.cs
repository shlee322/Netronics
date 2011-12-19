using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Netronics
{
    interface PacketDecoder
    {
        object decode(PacketBuffer buffer);
    }
}
