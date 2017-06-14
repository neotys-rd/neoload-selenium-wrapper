using Microsoft.VisualStudio.TestTools.UnitTesting;
using NeoLoadSelenium.neoload;
using NeoLoadSelenium.neoload.config;
using NeoLoadSelenium.neoload.interceptor;
using NeoLoadSelenium.neoload.wrapper;
using OpenQA.Selenium;
using OpenQA.Selenium.Remote;
using System.Collections.Generic;

namespace NeoLoadSelenium.tests.unit
{
    [TestClass]
    // A selenium server must be running. http://docs.seleniumhq.org/docs/03_webdriver.jsp#running-standalone-selenium-server-for-use-with-remotedrivers
    public class AdvancedValuesTest
    {
        [TestMethod]
        public void testGetAdvancedValuesHtmlUnit()
        {
            IWebDriver htmlUnitDriver = new RemoteWebDriver(DesiredCapabilities.HtmlUnit());
            NLWebDriver webDriver = NLWebDriverFactory.NewNLWebDriver(htmlUnitDriver);

            try
            {
                IDictionary <string, long> advancedValuesHU = WebDrivers.GetAdvancedValues(webDriver, ConfigurationHelper.newEUEConfiguration("", ""));
                Assert.IsTrue(advancedValuesHU.Count == 0); // There should be no values for the html unit driver.
            }
            finally
            {
                htmlUnitDriver.Quit();
            }
        }
    }
}
