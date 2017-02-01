using NeoLoadSelenium.neoload.config;
using OpenQA.Selenium;
using Castle.DynamicProxy;

namespace NeoLoadSelenium.neoload.interceptor.eue_element
{
    public class EUETargetLocatorProxy
    {
        private static string[] ALWAYS_SEND = { };
        private static string[] ON_EXCEPTION = { "Frame", "ParentFrame", "Window", "DefaultContent", "ActiveElement", "Alert" };
        private static string[] SET_LAST = { };
        private static EUEInterceptedMethods METHODS = new EUEInterceptedMethods(ALWAYS_SEND, ON_EXCEPTION, SET_LAST);

        private EUEElementInterceptor interceptor;
        
        private IWebDriver driver;
        private ITargetLocator targetLocator;
        private EUEEntryHandler dataExchangeEntryHelper;
        private EUEConfiguration conf;
        
        public EUETargetLocatorProxy(IWebDriver driver, ITargetLocator targetLocator, EUEEntryHandler dataExchangeEntryHelper, EUEConfiguration conf)
        {
            this.driver = driver;
            this.targetLocator = targetLocator;
            this.dataExchangeEntryHelper = dataExchangeEntryHelper;
            this.conf = conf;
            interceptor = new EUEElementInterceptor(driver, dataExchangeEntryHelper, conf, METHODS);
        }

        public ITargetLocator getProxy()
        {
            ProxyGenerator generator = new ProxyGenerator(new PersistentProxyBuilder());
            ITargetLocator proxy = generator.CreateInterfaceProxyWithTarget(typeof(ITargetLocator), targetLocator, interceptor) as ITargetLocator;
            return proxy;
        }
    }
}
