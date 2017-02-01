namespace NeoLoadSelenium.neoload.wrapper
{
    interface ICustomEUEConfigurator
    {
        /// <summary>
        /// Set the next name that will be used when data is sent to the Data Exchange server. This name is used once and then forgotten.
        /// </summary>
        /// <param name="name">name</param>
        void SetCustomName(string name);

        /// <summary>
        /// 
        /// </summary>
        /// <returns>the regular expression that's being used to clean URLs</returns>
        string GetRegexToCleanURLs();
    }
}
