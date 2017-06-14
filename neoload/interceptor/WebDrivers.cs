using NeoLoadSelenium.neoload.common;
using NeoLoadSelenium.neoload.config;
using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;

namespace NeoLoadSelenium.neoload.interceptor
{
    public class WebDrivers
    {
        private static bool? navigationTimingEnabled = null;
        // Helps us decide whether to send navigation timing values or not.
        private static string navigationTimingPreviousDomain = "";
        private static  IDictionary<string, long> navigationTimingPreviousAdvancedValues = new Dictionary<string, long>();

        public static string GetURL(IWebDriver driver)
        {
            try
            {
                return driver.Url;
            }
            catch
            {
                return "";
            }
        }

        public static string GetTitle(IWebDriver driver)
        {
            try
            {
                return driver.Title;
            }
            catch
            {
                return "";
            }
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public static IDictionary<string, long> GetAdvancedValues(IWebDriver webDriver, EUEConfiguration conf)
        {
            if(navigationTimingEnabled == null)
            {
                navigationTimingEnabled = conf.NavigationTimingEnabled;
            }

            if (navigationTimingEnabled == false || !(webDriver is IJavaScriptExecutor))
            {
                return new Dictionary<string, long>();
            }

            IDictionary<string, long> advancedValues = new Dictionary<string, long>();

            try
            {
                IJavaScriptExecutor javascriptExecutor = (IJavaScriptExecutor)webDriver;
                object redirectStart = javascriptExecutor.ExecuteScript("return window.performance.timing.redirectStart");
                object fetchStart = javascriptExecutor.ExecuteScript("return window.performance.timing.fetchStart");
                object responseStart = javascriptExecutor.ExecuteScript("return window.performance.timing.responseStart");
                object domContentLoadedEventStart = javascriptExecutor.ExecuteScript("return window.performance.timing.domContentLoadedEventStart");
                object domLoadEventStart = javascriptExecutor.ExecuteScript("return window.performance.timing.loadEventStart");
                object domLoadEventEnd = javascriptExecutor.ExecuteScript("return window.performance.timing.loadEventEnd");

                long start = GetStart(redirectStart, fetchStart);

                object timeToFirstByte = SafeSubtract(responseStart, start);
                object domContentLoaded = SafeSubtract(domContentLoadedEventStart, start);
                object onLoad = SafeSubtract(domLoadEventStart, start);
                object documentComplete = SafeSubtract(domLoadEventEnd, start);

                // the key is the label that appears in NeoLoad. A slash (/) adds another path element.
                PutIfNotNull(advancedValues, "Time To First Byte", timeToFirstByte);
                PutIfNotNull(advancedValues, "DOM Content Loaded", domContentLoaded);
                PutIfNotNull(advancedValues, "On Load", onLoad);
                PutIfNotNull(advancedValues, "Document Complete", documentComplete);
            }
            catch (Exception e)
            {
                if (conf.IsDebugEnabled)
                {
                    Logger.GetLogger().DebugMessage("Exception using navigating timing with " + webDriver.GetType().Name + ": " + e.Message);
                }
                // if anything ever goes wrong then give up.
                navigationTimingEnabled = false;
                return new Dictionary<string, long>();
            }

            // make sure we're not sending exactly the same values as last time.
            String currentDomain = GetDomainName(webDriver.Url);
            if (navigationTimingPreviousDomain == currentDomain && AdvancedValuesAreEqual(navigationTimingPreviousAdvancedValues, advancedValues))
            {
                // nothing has changed so don't send the same data over again.
                return new Dictionary<string, long>();
            }

            navigationTimingPreviousDomain = currentDomain;
            navigationTimingPreviousAdvancedValues = advancedValues;

            return advancedValues;
        }

        private static long GetStart(object redirectStart, object fetchStart)
        {
            if (redirectStart == null || (long)redirectStart == 0)
            {
                return (long)fetchStart;
            }
            return (long)redirectStart;
        }

        private static object SafeSubtract(object val1, object val2)
        {
            if (val1 == null || val2 == null)
            {
                return null;
            }

            return (long)val1 - (long)val2;
        }

        private static void PutIfNotNull(IDictionary<string, long> map, string key, object value)
        {
            if (value == null)
            {
                return;
            }
            map.Add(key, (long)value);
        }

        private static string GetDomainName(string urlAsString)
        {
            Uri uri;
            try
            {
                uri = new Uri(urlAsString);
                string domain = uri.Host == null ? "" : uri.Host;
                return domain.StartsWith("www.") ? domain.Substring(4) : domain;
            }
            catch (Exception e)
            {
                // don't care.
                return "";
            }
        }

        private static bool AdvancedValuesAreEqual(IDictionary<string, long> values1, IDictionary<string, long> values2)
        {
            if (values1.Values.Count != values2.Values.Count)
            {
                return false;
            }

            foreach (KeyValuePair<string, long> pair1 in values1.ToList())
            {
                if (!values2.ContainsKey(pair1.Key))
                {
                    return false;
                }

                long values2Value = values2[pair1.Key];

                if (pair1.Value != values2Value)
                {
                    return false;
                }
            }

            return true;
        }
    }
}
