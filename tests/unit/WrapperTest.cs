using Microsoft.VisualStudio.TestTools.UnitTesting;
using NeoLoadSelenium.neoload;
using NeoLoadSelenium.neoload.wrapper;
using OpenQA.Selenium;
using OpenQA.Selenium.Interactions;
using OpenQA.Selenium.Remote;

namespace NeoLoadSelenium.tests.unit
{
    [TestClass]
    // A selenium server must be running. http://docs.seleniumhq.org/docs/03_webdriver.jsp#running-standalone-selenium-server-for-use-with-remotedrivers
    public class WrapperTest
    {
        [TestMethod]
        public void testNewInstance()
        {
            IWebDriver webDriver = new RemoteWebDriver(DesiredCapabilities.HtmlUnit());
            try
            {
                NLWebDriver driver = NLWebDriverFactory.NewNLWebDriver(webDriver);
                Assert.IsNotNull(driver);
            }
            finally
            {
                webDriver.Close();
            }
        }

        [TestMethod]
        public void testGetRegexToCleanURLs()
        {
            IWebDriver webDriver = new RemoteWebDriver(DesiredCapabilities.HtmlUnit());
            try
            {
                NLWebDriver driver = NLWebDriverFactory.NewNLWebDriver(webDriver);
                string regexToCleanURLs = driver.GetRegexToCleanURLs();
                Assert.IsNotNull(regexToCleanURLs);
            }
            finally
            {
                webDriver.Close();
            }
        }


        [TestMethod]
        public void testStartTransaction()
        {
            IWebDriver webDriver = new RemoteWebDriver(DesiredCapabilities.HtmlUnit());
            try
            {
                NLWebDriver driver = NLWebDriverFactory.NewNLWebDriver(webDriver);
                // this should do nothing but not throw an exception.
                driver.StartTransaction("name");
            }
            finally
            {
                webDriver.Close();
            }
        }

        /**
         * Assure that HasInputDevices and other random interfaces are where they need to be.
         */
        [TestMethod]
        public void testHasInputDevices()
        {
            IWebDriver webDriver = new RemoteWebDriver(DesiredCapabilities.HtmlUnit());
            try
            {
                NLWebDriver driver = NLWebDriverFactory.NewNLWebDriver(webDriver);
                Actions builder = new Actions(driver);
                builder.MoveToElement(driver.FindElement(By.XPath("//*"))).Build().Perform();
                Assert.IsNotNull(builder);
            }
            finally
            {
                webDriver.Close();
            }
        }
    }
}
