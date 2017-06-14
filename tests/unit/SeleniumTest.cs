using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Remote;

namespace NeoLoadSelenium.tests.unit
{
    class SeleniumTest
    {
        IWebDriver driver;

        [SetUp]
        public void Initialize()
        {
            driver = new RemoteWebDriver(DesiredCapabilities.HtmlUnit());
        }

        [Test]
        public void OpenAppTest()
        {
            driver.Url = "http://www.demoqa.com";
        }

        [TearDown]
        public void EndTest()
        {
            driver.Close();
            driver.Quit();
        }
    }
}
