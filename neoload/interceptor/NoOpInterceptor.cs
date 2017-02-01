using System;
using OpenQA.Selenium;

namespace NeoLoadSelenium.neoload.interceptor
{
    class NoOpInterceptor : INeoLoadInterceptor
    {
        public void AfterSetUrl()
        {
            // nothing to do
        }

        public void BeforeSetUrl()
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

        public string DoOnGetRegexToCleanURLs()
        {
            // nothing to do
            return "";
        }

        public void DoOnQuit()
        {
            // nothing to do
        }

        public void DoOnSetCustomName(string customName)
        {
            // nothing to do
        }

        public void DoOnSetUrlWithException(WebDriverException e, string url)
        {
            // nothing to do
        }

        public void DoOnStart()
        {
            // nothing to do
        }

        public void DoOnStartTransaction(string name)
        {
            // nothing to do
        }

        public void DoOnStopTransaction()
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
