using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Service.Coroutine
{
    class NonNestedYield : IYield
    {
        public bool Resume()
        {
            return false;
        }
    }

}
