using NeoLoadSelenium.neoload;
using NeoLoadSelenium.neoload.wrapper;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.Remote;
using System;

namespace NeoLoadSelenium.tests.integration
{
    class IntegrationDesignTest
    {
        NLWebDriver driver;

        [SetUp]
        public void Initialize()
        {
            Environment.SetEnvironmentVariable("nl.selenium.proxy.mode", "Design");
            Environment.SetEnvironmentVariable("nl.design.api.url", "http://localhost:7400/Design/v1/Service.svc/");

            var webDriver = new RemoteWebDriver(DesiredCapabilities.HtmlUnitWithJavaScript());

            string projectPath = "C:\\Users\\anouvel\\Documents\\NeoLoad Projects\\v6.0\\Sample_Project\\Sample_Project.nlp";

            driver = NLWebDriverFactory.NewNLWebDriver(webDriver, "Selenium", projectPath);

        }

        [Test]
        public void OpenAppTest()
        {
            driver.StartTransaction("home");
            driver.Url = "http://ushahidi.demo.neotys.com/";

            driver.StartTransaction("reports");
            driver.FindElement(By.Id("mainmenu")).FindElements(By.TagName("a"))[1].Click();

            driver.StartTransaction("end");
            driver.Url = "http://ushahidi.demo.neotys.com/" + "/main";
        }

        [TearDown]
        public void EndTest()
        {
            driver.Close();
            driver.Quit();
        }
    }
}
