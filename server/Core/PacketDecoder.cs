﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Netronics
{
    public interface PacketDecoder
    {
        dynamic decode(PacketBuffer buffer);
    }
}
