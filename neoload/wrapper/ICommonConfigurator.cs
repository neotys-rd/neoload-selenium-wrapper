using System;

namespace NeoLoadSelenium.neoload.wrapper
{
    interface ICommonConfigurator
    {
        /// <summary>
        /// In Design mode, set the name of the current Transaction in NeoLoad.
        /// In EndUserExperience mode:
        /// - a timer is started, it will be stopped and sent to NeoLoad at next call of method stopTransaction or startTransaction.
        /// - the name of the transaction is added to the path of next entries send to NeoLoad.
        /// </summary>
        /// <param name="name">the name of the Transaction</param>
        void StartTransaction(string name);

        /// <summary>
        /// In Design mode, no operation is performed.
        /// In EndUserExperience mode:
        /// - a timer started with <code>startTransaction</code> is stopped and value is sent to NeoLoad.
        /// - the name of the transaction is not added to the path of next entries send to NeoLoad.
        /// </summary>
        void StopTransaction();
    }
}
