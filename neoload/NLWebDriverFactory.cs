﻿using NeoLoadSelenium.neoload.wrapper;
using System;
using OpenQA.Selenium;
using OpenQA.Selenium.Remote;
using NeoLoadSelenium.neoload.config;
using NeoLoadSelenium.neoload.interceptor;
using OpenQA.Selenium.Chrome;

namespace NeoLoadSelenium.neoload
{
    public class NLWebDriverFactory
    {

        private static string getDomainName(string url)
        {
            Uri uri;
            try
            {
                uri = new Uri(url);
                String domain = uri.Host;
                return domain.StartsWith("www.") ? domain.Substring(4) : domain;
            }
            catch (SystemException ex)
            {
                Console.WriteLine(ex.ToString());
                return "localhost";
            }
        }

        /**
                 * If the design mode is chosen then the driver will use NeoLoad as Proxy.
                 */
        public static DesiredCapabilities AddProxyCapabilitiesIfNecessary(DesiredCapabilities capabilities)
        {
            if (!Mode.DESIGN.Equals(ModeHelper.getMode()))
            {
                return capabilities;
            }

            DesignConfiguration conf = ConfigurationHelper.newDesignConfiguration(null, null);
            string host = getDomainName(conf.DesignAPIUrl);
            int port;
            try
            {
                port = DesignManager.newDesignAPIClientFromConfig(conf).GetRecorderSettings().ProxySettings.Port;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
                throw e;
            }
            string proxyString = host + ":" + port;

            Proxy proxy = new Proxy();
            proxy.HttpProxy = proxyString;
            proxy.SslProxy = proxyString;

            capabilities.SetCapability(CapabilityType.Proxy, proxy);
            capabilities.SetCapability(CapabilityType.AcceptSslCertificates, true);
            return capabilities;
        }

        /**
         * If the design mode is chosen then the driver will use NeoLoad as Proxy.
         */
        public static void AddProxyCapabilitiesIfNecessary(DriverOptions options)
        {
            if (!Mode.DESIGN.Equals(ModeHelper.getMode()))
            {
                return;
            }

            DesignConfiguration conf = ConfigurationHelper.newDesignConfiguration(null, null);
            string host = getDomainName(conf.DesignAPIUrl);
            int port;
            try
            {
                port = DesignManager.newDesignAPIClientFromConfig(conf).GetRecorderSettings().ProxySettings.Port;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
                throw e;
            }
            string proxyString = host + ":" + port;

            Proxy proxy = new Proxy();
            proxy.HttpProxy = proxyString;
            proxy.SslProxy = proxyString;

            if (options is ChromeOptions)
            {
                (options as ChromeOptions).Proxy = proxy;
            }
            else
            {
                options.AddAdditionalCapability(CapabilityType.Proxy, proxy);
                options.AddAdditionalCapability(CapabilityType.AcceptSslCertificates, true);
            }
            return;
        }

        /// <summary>
        /// Create a NeoLoad instance to the webDriver.
        /// In Design mode, the default User Path will be created or updated if it is already present in opened project.
        /// </summary>
        public static NLWebDriver NewNLWebDriver(IWebDriver webDriver)
        {
            return NewNLWebDriver(webDriver, null, null, new ParamBuilderProvider());
        }

        /// <summary>
        /// Create a NeoLoad instance to the webDriver.
        /// In Design mode, the userPath in parameter will be created or updated if it is already present in opened project.
        /// </summary>
        public static NLWebDriver NewNLWebDriver(IWebDriver webDriver, string userPath)
        {
            return NewNLWebDriver(webDriver, userPath, null, new ParamBuilderProvider());
        }

        /// <summary>
        /// Create a NeoLoad instance to the webDriver.
        /// In Design mode, project in parameter will be opened and the userPath in parameter will be created or updated if it is already present in opened project.
        /// </summary>
        public static NLWebDriver NewNLWebDriver(IWebDriver webDriver, string userPath, string projectPath)
        {
            return NewNLWebDriver(webDriver, userPath, projectPath, new ParamBuilderProvider());
        }

        /// <summary>
        /// Create a NeoLoad instance to the webDriver.
        /// In Design mode, project in parameter will be opened and the userPath in parameter will be created or updated if it is already present in opened project.
        /// ParamBuilderProvider can be overriden in order to update parameters.
        /// </summary>
        public static NLWebDriver NewNLWebDriver(IWebDriver webDriver, string userPath, string projectPath,
                                                 ParamBuilderProvider paramBuilderProvider)
        {
            Mode mode = ModeHelper.getMode();
            INeoLoadInterceptor interceptor;
            switch (mode)
            {
                case Mode.END_USER_EXPERIENCE:
                    var eueConf = ConfigurationHelper.newEUEConfiguration(webDriver.GetType().Name);
                    interceptor = new WebDriverEUEInterceptor(webDriver, eueConf);
                    break;
                case Mode.DESIGN:
                    var designConf = ConfigurationHelper.newDesignConfiguration(userPath, projectPath);
                    interceptor = new WebDriverDesignInterceptor(designConf, paramBuilderProvider);
                    break;
                case Mode.NO_API:
                default:
                    interceptor = new NoOpInterceptor();
                    break;
            }
            interceptor.DoOnStart();
            return new NLWebDriver(webDriver, interceptor);
        }

        private NLWebDriverFactory()
        {
        }
    }
}