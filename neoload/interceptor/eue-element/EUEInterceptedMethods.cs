using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NeoLoadSelenium.neoload.interceptor
{
    public class EUEInterceptedMethods
    {
        public List<string> alwaysSend { get; }
        public List<string> onException { get; }
        public List<string> setLast { get; }

        public EUEInterceptedMethods(string[] alwaysSend, string[] onException, string[] setLast)
        {
            this.alwaysSend = new List<string>(alwaysSend);
            this.onException = new List<string>(onException);
            this.setLast = new List<string>(setLast);
        }
    }
}
