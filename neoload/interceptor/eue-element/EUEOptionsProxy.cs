using Castle.DynamicProxy;
using NeoLoadSelenium.neoload.config;
using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NeoLoadSelenium.neoload.interceptor.eue_element
{
    class EUEOptionsProxy
    {
        private static string[] ALWAYS_SEND = { };
        private static string[] ON_EXCEPTION = { "AddCookie", "DeleteCookieNamed", "DeleteCookie",
                                                 "DeleteAllCookies", "AllCookies", "GetCookieNamed" };
        private static string[] SET_LAST = { };
        private static EUEInterceptedMethods METHODS = new EUEInterceptedMethods(ALWAYS_SEND, ON_EXCEPTION, SET_LAST);

        private EUEElementInterceptor interceptor;

        private IWebDriver driver;
        private IOptions options;
        private EUEEntryHandler dataExchangeEntryHelper;
        private EUEConfiguration conf;

        public EUEOptionsProxy(IWebDriver driver, IOptions options, EUEEntryHandler dataExchangeEntryHelper, EUEConfiguration conf)
        {
            this.driver = driver;
            this.options = options;
            this.dataExchangeEntryHelper = dataExchangeEntryHelper;
            this.conf = conf;
            interceptor = new EUEElementInterceptor(driver, dataExchangeEntryHelper, conf, METHODS);
        }

        public IOptions getProxy()
        {
            return new ForwardedOptions(interceptor, options);
        }

        private class ForwardedOptions : IOptions
        {

            private EUEElementInterceptor interceptor;
            private IOptions original;

            public ForwardedOptions(EUEElementInterceptor interceptor, IOptions original)
            {
                this.interceptor = interceptor;
                this.original = original;
            }

            public ICookieJar Cookies
            {
                get
                {
                    ProxyGenerator generator = new ProxyGenerator(new PersistentProxyBuilder());
                    ICookieJar proxy = generator.CreateInterfaceProxyWithTarget(typeof(ICookieJar), original.Cookies, interceptor) as ICookieJar;
                    return proxy;
                }
            }

            public ILogs Logs
            {
                get
                {
                    return original.Logs;
                }
            }

            public IWindow Window
            {
                get
                {
                    return original.Window;
                }
            }

            public ITimeouts Timeouts()
            {
                return original.Timeouts();
            }
        }
    }
}
