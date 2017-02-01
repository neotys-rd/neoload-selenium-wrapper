using Castle.DynamicProxy;
using NeoLoadSelenium.neoload.config;
using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NeoLoadSelenium.neoload.interceptor.eue_element
{
    public class EUEKeyboardProxy
    {
        private static string[] ALWAYS_SEND = { };
        private static string[] ON_EXCEPTION = { "PressKey", "ReleaseKey", "SendKeys" };
        private static string[] SET_LAST = { };
        private static EUEInterceptedMethods METHODS = new EUEInterceptedMethods(ALWAYS_SEND, ON_EXCEPTION, SET_LAST);

        private EUEElementInterceptor interceptor;

        private IWebDriver driver;
        private IKeyboard keyboard;
        private EUEEntryHandler dataExchangeEntryHelper;
        private EUEConfiguration conf;

        public EUEKeyboardProxy(IWebDriver driver, IKeyboard keyboard, EUEEntryHandler dataExchangeEntryHelper, EUEConfiguration conf)
        {
            this.driver = driver;
            this.keyboard = keyboard;
            this.dataExchangeEntryHelper = dataExchangeEntryHelper;
            this.conf = conf;
            interceptor = new EUEElementInterceptor(driver, dataExchangeEntryHelper, conf, METHODS);
        }

        public IKeyboard getProxy()
        {
            ProxyGenerator generator = new ProxyGenerator(new PersistentProxyBuilder());
            IKeyboard proxy = generator.CreateInterfaceProxyWithTarget(typeof(IKeyboard), keyboard, interceptor) as IKeyboard;
            return proxy;
        }
    }
}