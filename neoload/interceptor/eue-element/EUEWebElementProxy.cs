using Castle.DynamicProxy;
using NeoLoadSelenium.neoload.config;
using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NeoLoadSelenium.neoload.interceptor.eue_element
{
    class EUEWebElementProxy
    {
        private static string[] ALWAYS_SEND = { "Click", "Submit" };
        private static string[] ON_EXCEPTION = { "FindElement", "FindElements" };
        private static string[] SET_LAST = { };
        private static EUEInterceptedMethods METHODS = new EUEInterceptedMethods(ALWAYS_SEND, ON_EXCEPTION, SET_LAST);

        private EUEElementInterceptor interceptor;

        private IWebDriver driver;
        private IWebElement element;
        private EUEEntryHandler dataExchangeEntryHelper;
        private EUEConfiguration conf;

        public EUEWebElementProxy(IWebDriver driver, IWebElement element, EUEEntryHandler dataExchangeEntryHelper, EUEConfiguration conf)
        {
            this.driver = driver;
            this.element = element;
            this.dataExchangeEntryHelper = dataExchangeEntryHelper;
            this.conf = conf;
            interceptor = new EUEElementInterceptor(driver, dataExchangeEntryHelper, conf, METHODS);
        }

        public IWebElement getProxy()
        {
            ProxyGenerator generator = new ProxyGenerator(new PersistentProxyBuilder());
            IWebElement proxy = generator.CreateInterfaceProxyWithTarget(typeof(IWebElement), new Type[1]{typeof(ILocatable)}, element, ProxyGenerationOptions.Default, interceptor) as IWebElement;
            return proxy;
        }
    }
}
