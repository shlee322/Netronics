using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Netronics
{
    interface PacketEncoder
    {
        PacketBuffer encode(dynamic data);
    }
}
