using NeoLoadSelenium.neoload.config;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NeoLoadSelenium.neoload.common
{
    class Logger
    {
        private static Logger LOGGER = new Logger();

        private Logger() {}

        public static Logger GetLogger()
        {
            return LOGGER;
        }

        public void DebugMessage(string message)
        {
            System.Diagnostics.Debug.WriteLine(message);
        }
    }
}
