using Microsoft.VisualStudio.TestTools.UnitTesting;
using NeoLoadSelenium.neoload.interceptor.eue_element;
using OpenQA.Selenium;
using OpenQA.Selenium.Remote;

namespace NeoLoadSelenium.tests.unit
{
    [TestClass]
    public class RemoteWebElementProxifierTest
    {
        [TestMethod]
        public void testProxify()
        {
            IWebElement remoteWebElement = new RemoteWebElement(null, "abcdef");
            object wrapped = EUEElementProxifier.proxifyIfRequired(remoteWebElement, null, null, null);

            Assert.IsTrue(wrapped  != remoteWebElement && wrapped is IWebElement); //should be an instance of IWebElement
        }
    }
}
