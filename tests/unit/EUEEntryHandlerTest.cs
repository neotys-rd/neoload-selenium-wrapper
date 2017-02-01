using Microsoft.VisualStudio.TestTools.UnitTesting;
using NeoLoadSelenium.neoload.config;
using NeoLoadSelenium.neoload.interceptor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NeoLoadSelenium.tests.unit
{
    [TestClass]
    public class EUEEntryHandlerTest
    {
        [TestMethod]
        public void shouldCleanUrlCorrectly()
        {
            // Java
            // cleanedURL = urlStr.substring(urlStr.indexOf(url.Path, url.getHost().length() + "//".length()));

            string urlStr = "http://docker01.intranet.neotys.com:54337/toto/titi.tyu.css";

            string urlStrTrimed = urlStr == null ? "" : urlStr.Trim();

            Uri url = new Uri(urlStrTrimed);

            string localPath = url.LocalPath;
            string host = url.Host;

            int index = urlStr.IndexOf(localPath, host.Length + "//".Length);
            string cleanedUrl = urlStr.Substring(index);

            Assert.AreEqual(cleanedUrl, "/toto/titi.tyu.css");
        }

        /// <summary>
        /// Make sure the path is split by slashes (/)
        /// </summary>
        [TestMethod]
        public void testCreatePathURLSplit()
        {

            List<string> path = EUEEntryHandler.createPath("current/url", "pageTitle", ConfigurationHelper.newEUEConfiguration(""));

            Assert.AreEqual(4, path.Count); // Fail if Path doesn't have enough parts.
            Assert.AreEqual("url", path[path.Count - 1]); // Fail if End of the path is wrong.
        }

        /// <summary>
        /// Make sure the path is split by slashes (/)
        /// </summary>
        [TestMethod]
        public void testCreatePathCustomNameSplit()
        {
            var conf = ConfigurationHelper.newEUEConfiguration("");
            conf.CurrentCustomName = "bob/cobb/custom/name";


            List<string> path = EUEEntryHandler.createPath("current/url", "pageTitle/something", conf);

            Assert.AreEqual(6, path.Count); // Fail if Path doesn't have enough parts.
            Assert.AreEqual("name", path[path.Count - 1]); // Fail if End of the path is wrong.
        }

        /** Make sure the path is split by slashes (/).
         * Test method for {@link com.neotys.selenium.proxies.helpers.EntryHandler#createPath(java.lang.String, java.lang.String)}.
         */
        [TestMethod]
        public void testCreatePathTitleSplit()
        {
            try
            {
                Environment.SetEnvironmentVariable("nl.path.naming.policy", "Title");

                List<string> path = EUEEntryHandler.createPath("current/url", "bob/pageTitle/something", ConfigurationHelper.newEUEConfiguration(""));

                Assert.AreEqual(5, path.Count); // Fail if Path doesn't have enough parts.
                Assert.AreEqual("something", path[path.Count - 1]); // Fail if End of the path is wrong.
            }
            finally
            {
                Environment.SetEnvironmentVariable("nl.path.naming.policy", "");
            }
        }
    }
}
