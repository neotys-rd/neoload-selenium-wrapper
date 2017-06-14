using NeoLoadSelenium.neoload.config;
using OpenQA.Selenium;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace NeoLoadSelenium.neoload.interceptor.eue_element
{
    public class EUEElementProxifier
    {
        public static object proxifyIfRequired(object original, IWebDriver driver, EUEEntryHandler dataExchangeEntryHelper, EUEConfiguration conf)
        {
            if(original is IMouse)
            {
                return new EUEMouseProxy(driver, original as IMouse, dataExchangeEntryHelper, conf).getProxy();
            }

            if(original is IKeyboard)
            {
                return new EUEKeyboardProxy(driver, original as IKeyboard, dataExchangeEntryHelper, conf).getProxy();
            }

            if(original is IOptions)
            {
                return new EUEOptionsProxy(driver, original as IOptions, dataExchangeEntryHelper, conf).getProxy();
            }

            if (original is ITargetLocator)
            {
                return new EUETargetLocatorProxy(driver, original as ITargetLocator, dataExchangeEntryHelper, conf).getProxy();
            }

            if (original is INavigation)
            {
                return new EUENavigationProxy(driver, original as INavigation, dataExchangeEntryHelper, conf).getProxy();
            }
            
            if (original is ReadOnlyCollection<IWebElement>)
            {
                ReadOnlyCollection<IWebElement> originalElements = original as ReadOnlyCollection<IWebElement>;
                List<IWebElement> proxified = new List<IWebElement>();
                foreach (IWebElement originalElement in originalElements)
                {
                    proxified.Add(proxifyIfRequired(originalElement, driver, dataExchangeEntryHelper, conf) as IWebElement);
                }
                return new ReadOnlyCollection<IWebElement>(proxified);
            }

            if (original is IWebElement)
            {
                return new EUEWebElementProxy(driver, original as IWebElement, dataExchangeEntryHelper, conf).getProxy() as IWebElement;
            }
            
            return original;
        }
    }
}
