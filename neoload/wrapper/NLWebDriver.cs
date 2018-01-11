using OpenQA.Selenium;
using System.Collections.ObjectModel;
using NeoLoadSelenium.neoload.interceptor;
using OpenQA.Selenium.Internal;
using OpenQA.Selenium.Interactions;
using System;
using System.Collections.Generic;

namespace NeoLoadSelenium.neoload.wrapper
{
    public class NLWebDriver : IWebDriver, ICustomEUEConfigurator, ICommonConfigurator, IHasInputDevices, IActionExecutor
    {
        private IWebDriver driver;
        private INeoLoadInterceptor interceptor;

        public NLWebDriver(IWebDriver driver, INeoLoadInterceptor interceptor)
        {
            this.driver = driver;
            this.interceptor = interceptor;
        }

        public string CurrentWindowHandle
        {
            get
            {
                return driver.CurrentWindowHandle;
            }
        }

        public string PageSource
        {
            get
            {
                return driver.PageSource;
            }
        }

        public string Title
        {
            get
            {
                return driver.Title;
            }
        }

        public string Url
        {
            get
            {
                return driver.Url;
            }

            set
            {
                try
                {
                    interceptor.BeforeSetUrl();
                    driver.Url = value;
                    interceptor.AfterSetUrl();
                }
                catch (OpenQA.Selenium.WebDriverException e)
                {
                    interceptor.DoOnSetUrlWithException(e, value);
                    throw e;
                }
            }
        }

        public ReadOnlyCollection<string> WindowHandles
        {
            get
            {
                return driver.WindowHandles;
            }
        }

        public IKeyboard Keyboard
        {
            get
            {
                if (driver is IHasInputDevices)
                {
                    return interceptor.proxify((driver as IHasInputDevices).Keyboard) as IKeyboard;
                }
                return null;
            }
        }

        public IMouse Mouse
        {
            get
            {
                if (driver is IHasInputDevices)
                {
                    return interceptor.proxify((driver as IHasInputDevices).Mouse) as IMouse;
                }
                return null;
            }
        }

        public bool IsActionExecutor
        {
            get
            {
                if (driver is IActionExecutor)
                {
                    return (driver as IActionExecutor).IsActionExecutor;
                }
                return false;
            }
        }

        public void StartTransaction(string name)
        {
            interceptor.DoOnStartTransaction(name);
        }

        public void StopTransaction()
        {
            interceptor.DoOnStopTransaction();
        }

        public void Close()
        {
            driver.Close();
        }

        public void Dispose()
        {
            driver.Dispose();
        }

        public IWebElement FindElement(By by)
        {
            try
            {
                interceptor.DoOnFindOneOrMoreElements("FindElement", by);
                return interceptor.proxify(driver.FindElement(by)) as IWebElement;
            }
            catch (OpenQA.Selenium.WebDriverException e)
            {
                interceptor.DoOnFindOneOrMoreElementsWithException("FindElement", by, e);
                throw e;
            }
        }

        public ReadOnlyCollection<IWebElement> FindElements(By by)
        {
            try
            {
                interceptor.DoOnFindOneOrMoreElements("FindElements", by);
                return interceptor.proxify(driver.FindElements(by)) as ReadOnlyCollection<IWebElement>;
            }
            catch (OpenQA.Selenium.WebDriverException e)
            {
                interceptor.DoOnFindOneOrMoreElementsWithException("FindElements", by, e);
                throw e;
            }
        }
        
        public string GetRegexToCleanURLs()
        {
            return interceptor.DoOnGetRegexToCleanURLs();
        }

        public IOptions Manage()
        {
            return interceptor.proxify(driver.Manage()) as IOptions;
        }

        public INavigation Navigate()
        {
            return interceptor.proxify(driver.Navigate()) as INavigation;
        }

        public void Quit()
        {
            driver.Quit();
            interceptor.DoOnQuit();
        }

        public void SetCustomName(string name)
        {
            interceptor.DoOnSetCustomName(name);
        }

        public ITargetLocator SwitchTo()
        {
            return interceptor.proxify(driver.SwitchTo()) as ITargetLocator;
        }

        public void PerformActions(IList<ActionSequence> actionSequenceList)
        {
            if (driver is IActionExecutor)
            {
                (driver as IActionExecutor).PerformActions(actionSequenceList);
            }
        }

        public void ResetInputState()
        {
            if (driver is IActionExecutor)
            {
                (driver as IActionExecutor).ResetInputState();
            }
        }
    }
}
