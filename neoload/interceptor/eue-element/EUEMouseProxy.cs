using NeoLoadSelenium.neoload.config;
using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenQA.Selenium.Interactions.Internal;
using Castle.DynamicProxy;

namespace NeoLoadSelenium.neoload.interceptor.eue_element
{
    public class EUEMouseProxy
    {
        private static string[] ALWAYS_SEND = { "Click", "DoubleClick", "ContextClick" };
        private static string[] ON_EXCEPTION = { };
        private static string[] SET_LAST = { };
        private static EUEInterceptedMethods METHODS = new EUEInterceptedMethods(ALWAYS_SEND, ON_EXCEPTION, SET_LAST);

        private EUEElementInterceptor interceptor;
        
        private IWebDriver driver;
        private IMouse mouse;
        private EUEEntryHandler dataExchangeEntryHelper;
        private EUEConfiguration conf;
        
        public EUEMouseProxy(IWebDriver driver, IMouse mouse, EUEEntryHandler dataExchangeEntryHelper, EUEConfiguration conf)
        {
            this.driver = driver;
            this.mouse = mouse;
            this.dataExchangeEntryHelper = dataExchangeEntryHelper;
            this.conf = conf;
            interceptor = new EUEElementInterceptor(driver, dataExchangeEntryHelper, conf, METHODS);
        }

        public IMouse getProxy()
        {
            ProxyGenerator generator = new ProxyGenerator(new PersistentProxyBuilder());
            IMouse proxy = generator.CreateInterfaceProxyWithTarget(typeof(IMouse), mouse, interceptor) as IMouse;
            return proxy;
        }
    }
}
