using NeoLoadSelenium.neoload.config;
using OpenQA.Selenium;
using Castle.DynamicProxy;

namespace NeoLoadSelenium.neoload.interceptor.eue_element
{
    public class EUENavigationProxy
    {
        private static string[] ALWAYS_SEND = { "GoToUrl" };
        private static string[] ON_EXCEPTION = { "Back", "Forward", "GoToUrl", "Refresh" };
        private static string[] SET_LAST = { };
        private static EUEInterceptedMethods METHODS = new EUEInterceptedMethods(ALWAYS_SEND, ON_EXCEPTION, SET_LAST);

        private EUEElementInterceptor interceptor;
        
        private IWebDriver driver;
        private INavigation navigation;
        private EUEEntryHandler dataExchangeEntryHelper;
        private EUEConfiguration conf;
        
        public EUENavigationProxy(IWebDriver driver, INavigation navigation, EUEEntryHandler dataExchangeEntryHelper, EUEConfiguration conf)
        {
            this.driver = driver;
            this.navigation = navigation;
            this.dataExchangeEntryHelper = dataExchangeEntryHelper;
            this.conf = conf;
            interceptor = new EUEElementInterceptor(driver, dataExchangeEntryHelper, conf, METHODS);
        }

        public INavigation getProxy()
        {
            ProxyGenerator generator = new ProxyGenerator(new PersistentProxyBuilder());
            INavigation proxy = generator.CreateInterfaceProxyWithTarget(typeof(INavigation), navigation, interceptor) as INavigation;
            return proxy;
        }
    }
}
