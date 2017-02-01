using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NeoLoadSelenium.neoload.common
{
    public class Objects
    {
        public static Object FirstNonNull(params Object[] values)
        {
            if (values != null)
            {
                foreach (Object val in values)
                {
                    if (val != null)
                    {
                        return val;
                    }
                }
            }
            return null;
        }
    }
}
