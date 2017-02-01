using System;

namespace NeoLoadSelenium.neoload.wrapper
{
    interface ICustomDesignConfigurator
    {
        /// <summary>
        /// Set the name of the current Transaction in NeoLoad.
        /// </summary>
        /// <param name="name"></param>
        void StartTransaction(String name);

        void StopTransaction();
    }
}
