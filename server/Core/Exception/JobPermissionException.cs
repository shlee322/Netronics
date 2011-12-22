using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Netronics.Exception
{
    class JobPermissionException : System.Exception
    {
        public JobPermissionException(String message)
            : base(message)
        {
        }
    }
}
