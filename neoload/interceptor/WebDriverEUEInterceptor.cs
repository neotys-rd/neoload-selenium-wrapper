using NeoLoadSelenium.neoload.common;
using NeoLoadSelenium.neoload.config;
using NeoLoadSelenium.neoload.interceptor.eue_element;
using Neotys.DataExchangeAPI.Client;
using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;

namespace NeoLoadSelenium.neoload.interceptor
{
    class WebDriverEUEInterceptor : INeoLoadInterceptor
    {
        private IWebDriver driver;
        private EUEConfiguration config;
        private EUEEntryHandler dataExchangeEntryHelper;

        public WebDriverEUEInterceptor(IWebDriver driver, EUEConfiguration config)
        {
            this.driver = driver;
            this.config = config;
            this.dataExchangeEntryHelper = new EUEEntryHandler(config);
        }

        public string DoOnGetRegexToCleanURLs()
        {
            return config.RegexToCleanURLs;
        }

        public void DoOnSetCustomName(string customName)
        {
            this.config.CurrentCustomName = customName;
        }

        public void DoOnFindOneOrMoreElements(string findElementTypeMethodName, By by)
        {
            DoOnMethodsRequiringToSetLastAction(findElementTypeMethodName, by);
            dataExchangeEntryHelper.ResetStartTime();
        }

        public void DoOnFindOneOrMoreElementsWithException(string findElementTypeMethodName, By by, WebDriverException e)
        {
            string newMessage = e.Message + " {\"method\":\"" + findElementTypeMethodName + "\",\"params\":\"" + by + "\"}";
            WebDriverException wrappedException = new WebDriverException(newMessage, e);

            dataExchangeEntryHelper.SendEntryThrow(WebDrivers.GetURL(driver), WebDrivers.GetTitle(driver), wrappedException, findElementTypeMethodName, WebDrivers.GetAdvancedValues(driver, config));
        }

        private void DoOnMethodsRequiringToSetLastAction(string methodName, object argument)
        {
            this.config.CurrentLastAction = methodName + " " + argument;
        }

        public void BeforeSetUrl()
        {
            dataExchangeEntryHelper.ResetStartTime();
        }

        public void AfterSetUrl()
        {
            dataExchangeEntryHelper.SendEntry(WebDrivers.GetURL(driver), WebDrivers.GetTitle(driver), "setUrl", WebDrivers.GetAdvancedValues(driver, config));
        }

        public void DoOnSetUrlWithException(WebDriverException e, string url)
        {
            string methodName = "setUrl";
            string newMessage = e.Message + " {\"method\":\"" + methodName + "\",\"params\":\"" + url + "\"}";
            WebDriverException wrappedException = new WebDriverException(newMessage, e);

            dataExchangeEntryHelper.SendEntryThrow(WebDrivers.GetURL(driver), WebDrivers.GetTitle(driver), wrappedException, methodName, WebDrivers.GetAdvancedValues(driver, config));
        }

        public void DoOnQuit()
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

        public object proxify(object webElement)
        {
            return EUEElementProxifier.proxifyIfRequired(webElement, driver, dataExchangeEntryHelper, config);
        }
    }
}
