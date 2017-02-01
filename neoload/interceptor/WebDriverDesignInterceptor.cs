using System;
using NeoLoadSelenium.neoload.config;
using OpenQA.Selenium;

namespace NeoLoadSelenium.neoload.interceptor
{
    class WebDriverDesignInterceptor : INeoLoadInterceptor
    {
        private DesignManager designManager;

        public WebDriverDesignInterceptor(DesignConfiguration config, ParamBuilderProvider paramBuilderProvider)
        {
            this.designManager = new DesignManager(config, paramBuilderProvider);
        }

        public void DoOnStart()
        {
            designManager.start();
        }

        public void DoOnStartTransaction(string name)
        {
            designManager.startTransaction(name);
        }

        public void DoOnStopTransaction()
        {
            // nothing to do
        }

        public void DoOnQuit()
        {
            designManager.stop();
        }

        public string DoOnGetRegexToCleanURLs()
        {
            // nothing to do
            return "";
        }

        public void DoOnSetCustomName(string customName)
        {
            // nothing to do
        }

        public void DoOnFindOneOrMoreElements(string methodName, object argument)
        {
            // nothing to do
        }

        public void DoOnFindOneOrMoreElements(string methodName, By by)
        {
            // nothing to do
        }

        public void DoOnFindOneOrMoreElementsWithException(string findElementTypeMethodName, By by, WebDriverException e)
        {
            // nothing to do
        }

        public void BeforeSetUrl()
        {
            // nothing to do
        }

        public void AfterSetUrl()
        {
            // nothing to do
        }

        public void DoOnSetUrlWithException(WebDriverException e, string url)
        {
            // nothing to do
        }

        public object proxify(object original)
        {
            // nothing to do
            return original;
        }
    }
}
