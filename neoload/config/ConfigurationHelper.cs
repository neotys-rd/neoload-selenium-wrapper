﻿using NUnit.Framework;
using OpenQA.Selenium;
using System;
using System.Diagnostics;
using System.Reflection;
using System.Text.RegularExpressions;

namespace NeoLoadSelenium.neoload.config
{
    class ConfigurationHelper
    {
        /// <summary>
        /// All variables begin with this prefix.
        /// </summary>
        static string OPT_PREFIX = "nl.";
        public static string OPT_SELENIUM_WRAPPER_MODE = OPT_PREFIX + "selenium.proxy.mode";
        static string OPT_DATA_EXCHANGE_URL = OPT_PREFIX + "data.exchange.url";
        static string OPT_DESIGN_API_URL = OPT_PREFIX + "design.api.url";
        static string OPT_DEBUG = OPT_PREFIX + "debug";
        static string OPT_API_KEY = OPT_PREFIX + "api.key";
        static string OPT_LOCATION = OPT_PREFIX + "location";
        static string OPT_SOFTWARE = OPT_PREFIX + "software";
        static string OPT_OS = OPT_PREFIX + "os";
        static string OPT_HARDWARE = OPT_PREFIX + "hardware";
        static string OPT_INSTANCE_ID = OPT_PREFIX + "instance.id";
        static string OPT_SCRIPT_NAME = OPT_PREFIX + "script.name";
        static string OPT_PATH_NAMING_POLICY = OPT_PREFIX + "path.naming.policy";
        static string OPT_REGEX_TO_CLEAN_URLS = OPT_PREFIX + "regex.to.clean.urls";
        static string OPT_NAVIGATION_TIMING = OPT_PREFIX + "navigation.timing.enabled";

        static string DEFAULT_DATA_EXCHANGE_URL = "http://localhost:7400/DataExchange/v1/Service.svc/";
        static string DEFAULT_DESIGN_URL = "http://localhost:7400/Design/v1/Service.svc/";

        static string DEFAULT_SCRIPT_NAME_PREFIX = "SeleniumDelegate";

        private static string OPT_CAPABILITIES_PREFIX = OPT_PREFIX + "capabilities.";

	    /** Environment variables used to override the key used to find device information in capabilities. */
	    public static string OPT_CAPABILITIES_PLATFORM_NAME = OPT_CAPABILITIES_PREFIX + "platform.name";
	    public static string OPT_CAPABILITIES_PLATFORM_VERSION = OPT_CAPABILITIES_PREFIX + "platform.version";
	    public static string OPT_CAPABILITIES_DEVICE_NAME = OPT_CAPABILITIES_PREFIX + "device.name";
	    public static string OPT_CAPABILITIES_BROWSER_NAME = OPT_CAPABILITIES_PREFIX + "browser.name";
	    public static string OPT_CAPABILITIES_BROWSER_VERSION = OPT_CAPABILITIES_PREFIX + "browser.version";
	    public static string OPT_CAPABILITIES_LOCATION = OPT_CAPABILITIES_PREFIX + "location";

        public static string getPropertyValue(string key, string defaultValue)
        {
            string envValue = Environment.GetEnvironmentVariable(key);
            if (envValue != null)
            {
                return envValue;
            }

            string pattern = "-D" + Regex.Escape(key) + "=" + "(.+)";
            foreach (string arg in Environment.GetCommandLineArgs())
            {
                MatchCollection matchCollection = Regex.Matches(arg, pattern);
                if (matchCollection.Count > 0)
                {
                    return matchCollection[0].Groups[1].Value;
                }
            }
            return defaultValue;
        }

        private static string getDefaultInstanceID()
        {
            return DateTime.Now.ToString("yyyy-MM-dd'T'HH:mm:ss");
        }

        private static PathNamingPolicy getPathNamingPolicy()
        {
            string policyString = getPropertyValue(OPT_PATH_NAMING_POLICY, null);
            return policyString == null ? PathNamingPolicy.URL : (PathNamingPolicy)Enum.Parse(typeof(PathNamingPolicy), policyString.ToUpper());
        }

        public static EUEConfiguration newEUEConfiguration(string driverType, string userPathName, ICapabilities capabilities)
        {
            string dataExchangeApiUrl = getPropertyValue(OPT_DATA_EXCHANGE_URL, DEFAULT_DATA_EXCHANGE_URL);
            string apiKey = getPropertyValue(OPT_API_KEY, "");
            bool isDebug = Boolean.Parse(getPropertyValue(OPT_DEBUG, "false"));
            string location = getPropertyValue(OPT_LOCATION, getLocation(capabilities));
            string regexToCleanURLs = getPropertyValue(OPT_REGEX_TO_CLEAN_URLS, "(.*?)[#?;%].*");
            string software = getPropertyValue(OPT_SOFTWARE, getSoftware(capabilities, driverType));
            string os = getPropertyValue(OPT_OS, getOs(capabilities, Environment.OSVersion.VersionString));
            string hardware = getPropertyValue(OPT_HARDWARE, getHardware(capabilities));
            string instanceID = getPropertyValue(OPT_INSTANCE_ID, getDefaultInstanceID());
            string scriptName = getPropertyValue(OPT_SCRIPT_NAME, null);
            PathNamingPolicy pathNamingPolicy = getPathNamingPolicy();
            bool navigationTimingEnabled = Boolean.Parse(getPropertyValue(OPT_NAVIGATION_TIMING, "true"));

            return new EUEConfiguration(apiKey, userPathName, dataExchangeApiUrl, isDebug, location, regexToCleanURLs, software, os,
                                        hardware, instanceID, scriptName, pathNamingPolicy, navigationTimingEnabled);
        }

        private static string getHardware(ICapabilities capabilities)
        {
            if (capabilities != null)
            {
                string capabilityName = getPropertyValue(OPT_CAPABILITIES_DEVICE_NAME, "deviceName");
                if (capabilities.HasCapability(capabilityName))
                {
                    return capabilities.GetCapability(capabilityName).ToString();
                }
            }
            return "";
        }

        private static string getOs(ICapabilities capabilities, string systemOs)
        {
            if (capabilities != null)
            {
                string platformNameCapability = getPropertyValue(OPT_CAPABILITIES_PLATFORM_NAME, "platformName");
                if (capabilities.HasCapability(platformNameCapability))
                {
                    string platformName = capabilities.GetCapability(platformNameCapability).ToString();
                    string platformVersionCapability = getPropertyValue(OPT_CAPABILITIES_PLATFORM_VERSION, "platformVersion");
                    if (capabilities.HasCapability(platformVersionCapability))
                    {
                        return platformName + " " + capabilities.GetCapability(platformVersionCapability).ToString();
                    }
                    return platformName;
                }
            }
            return systemOs;
        }

        private static string getSoftware(ICapabilities capabilities, string driverType)
        {
            if (capabilities != null)
            {
                string browserNameCapability = getPropertyValue(OPT_CAPABILITIES_BROWSER_NAME, "browserName");
                if (capabilities.HasCapability(browserNameCapability))
                {
                    string browserName = capabilities.GetCapability(browserNameCapability).ToString();
                    string browserVersionCapability = getPropertyValue(OPT_CAPABILITIES_BROWSER_VERSION, "browserVersion");
                    if (capabilities.HasCapability(browserVersionCapability))
                    {
                        return browserName + " " +capabilities.GetCapability(browserVersionCapability).ToString();
                    }
                    return browserName;
                }
            }
            return "";
        }

        private static string getLocation(ICapabilities capabilities)
        {
            if (capabilities != null)
            {
                string capabilityName = getPropertyValue(OPT_CAPABILITIES_LOCATION, "location");
                if (capabilities.HasCapability(capabilityName))
                {
                    return capabilities.GetCapability(capabilityName).ToString();
                }
            }
            return "";
        }

        public static string getUnitTestName()
        {
            string testMethodName = null;
            if (TestContext.CurrentContext != null && TestContext.CurrentContext.Test != null && TestContext.CurrentContext.Test.MethodName != null)
            {
                testMethodName = DEFAULT_SCRIPT_NAME_PREFIX + "-" + TestContext.CurrentContext.Test.Name;
            }

            if(testMethodName == null || testMethodName.Equals(""))
            {
                foreach (var stackFrame in new StackTrace().GetFrames())
                {
                    MethodBase methodBase = stackFrame.GetMethod();
                    Object[] attributes = methodBase.GetCustomAttributes(typeof(TestAttribute), false);
                    if (attributes.Length >= 1)
                    {
                        return DEFAULT_SCRIPT_NAME_PREFIX + "-" + methodBase.Name;
                    }
                }
                testMethodName = DEFAULT_SCRIPT_NAME_PREFIX;
            }
            return testMethodName;
        }

        public static DesignConfiguration newDesignConfiguration(string userPathName, string projectPath)
        {
            string designApiUrl = getPropertyValue(OPT_DESIGN_API_URL, DEFAULT_DESIGN_URL);
            string apiKey = getPropertyValue(OPT_API_KEY, "");
            return new DesignConfiguration(apiKey, designApiUrl, userPathName, projectPath);
        }
    }
}
