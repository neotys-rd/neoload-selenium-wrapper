using NeoLoadSelenium.neoload;
using NeoLoadSelenium.neoload.wrapper;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.Remote;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NeoLoadSelenium.tests.integration
{
    class IntegrationEUETest
    {
        NLWebDriver driver;

        [SetUp]
        public void Initialize()
        {
            Environment.SetEnvironmentVariable("nl.selenium.proxy.mode", "EndUserExperience");
            Environment.SetEnvironmentVariable("nl.data.exchange.url", "http://localhost:7400/DataExchange/v1/Service.svc/");
            Environment.SetEnvironmentVariable("nl.api.key", "key");

            var webDriver = new RemoteWebDriver(DesiredCapabilities.HtmlUnitWithJavaScript());

            string projectPath = "C:\\Users\\dregnier\\Documents\\NeoLoad Projects\\v5.3\\Sample_Project\\Sample_Project.nlp";

            driver = NLWebDriverFactory.NewNLWebDriver(webDriver, "SeleniumC-Sharp", projectPath);
        }

        [Test]
        public void OpenAppTest()
        {
            
            // check it does not impact EUE
            driver.StartTransaction("home1");

            driver.Url = "http://ushahidi.demo.neotys.com/";

            driver.SetCustomName("C-SHARP-1");

            IWebElement firstFind = driver.FindElement(By.Id("mainmenu"));

            IWebElement secondFind = firstFind.FindElements(By.TagName("a")).First();
            secondFind.Click();
            //try
            //{
            //    IWebElement thirdFind = driver.FindElement(By.PartialLinkText("SUBMIT"));
            //    thirdFind.Click();
            //}
            //catch (Exception e)
            //{
            //    // do nothing
            //}

            driver.SetCustomName("C-SHARP-2");

            driver.Url = "http://ushahidi.demo.neotys.com/";
            
            driver.Url = "http://ushahidi.demo.neotys.com/reports";

            driver.StopTransaction();
        }

        [TearDown]
        public void EndTest()
        {
            driver.Close();
            driver.Quit();
        }
    }
}
