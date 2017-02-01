using NeoLoadSelenium.neoload.common;
using NeoLoadSelenium.neoload.config;
using Neotys.DataExchangeAPI.Client;
using Neotys.DataExchangeAPI.Model;
using Neotys.DataExchangeAPI.Rest.Util;
using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;

namespace NeoLoadSelenium.neoload.interceptor
{
    public class EUEEntryHandler
    {
        private EUEConfiguration conf;
        private long startTime;
        private WebDriverException exception = null;

        private IDataExchangeAPIClient dataExchangeAPIClient;

        private bool isDebug;

        public EUEEntryHandler(EUEConfiguration conf)
        {
            this.conf = conf;
            startTime = Time.CurrentTimeMillis();
            dataExchangeAPIClient = NewClient(conf);
            isDebug = conf.IsDebugEnabled;
        }

        public void ResetStartTime()
        {
            startTime = Time.CurrentTimeMillis();
        }

        private static IDataExchangeAPIClient NewClient(EUEConfiguration conf)
        {
            ContextBuilder cb = new ContextBuilder();
            cb.Software = conf.Software;
            cb.Os = conf.Os;
            cb.Hardware = conf.Hardware;
            cb.InstanceId = conf.InstanceId;
            cb.Location = conf.Location;
            cb.Script = conf.ScriptName;
            Context context = cb.build();

            string dataExchangeAPIURL = conf.DataExchangeAPIUrl;
            string dataExchangeAPIKey = conf.ApiKey;

            if (conf.IsDebugEnabled)
            {
                Logger.GetLogger().DebugMessage(
                    "Connecting to data exchange API server. " +
                    "URL: " + dataExchangeAPIURL +
                    ", API key: " + dataExchangeAPIKey + 
                    ", Context: " + context);
            }

            if (dataExchangeAPIKey != null && dataExchangeAPIKey != "")
            {
                return DataExchangeAPIClientFactory.NewClient(conf.DataExchangeAPIUrl, context, dataExchangeAPIKey);
            }
            return DataExchangeAPIClientFactory.NewClient(conf.DataExchangeAPIUrl, context, "");
        }

        public void SendEntryThrow(string currentURL, string pageTitle, WebDriverException e, string methodName, IDictionary<string, long> advancedValues)
        {
            exception = e;
            SendEntry(currentURL, pageTitle, methodName, advancedValues);
        }

        public void SendEntry(string currentURL, string pageTitle, string methodName, IDictionary<string, long> advancedValues)
        {
            sendData(currentURL, pageTitle, methodName, advancedValues);
            throwStoredException();
        }

        void sendData(string currentURL, string pageTitle, string methodName, IDictionary<string, long> advancedValues)
        {
            // set the data.
            List<string> entryPath = createPath(currentURL, pageTitle, conf);
            EntryBuilder ebNormal = new EntryBuilder(entryPath, startTime);
            ebNormal.Url = currentURL;

            ebNormal.Status = Statuses.NewStatus(methodName, exception);
		    double value = Time.CurrentTimeMillis() - startTime;
            ebNormal.Value = value;
            ebNormal.Unit = TimerBuilder.DefaultUnit;

		    List<Entry> entriesToSend = new List<Entry>();
            entriesToSend.Add(ebNormal.Build());
		
		    foreach(KeyValuePair<string, long> pair in advancedValues.ToList()) {
		        List<string> advancedPath = new List<string>(entryPath);

                string[] pathEntries = pair.Key.Split('/');
                advancedPath.AddRange(pathEntries);

		        EntryBuilder ebAdv = new EntryBuilder(advancedPath, startTime);
                ebAdv.Url = currentURL;
		        ebAdv.Status = Statuses.NewStatus(methodName, exception);
		        ebAdv.Value = value;
	            ebAdv.Unit = TimerBuilder.DefaultUnit;
	            ebAdv.Value = pair.Value;
		    
		        entriesToSend.Add(ebAdv.Build());
		    }

            if (isDebug)
            {
                foreach (Entry entry in entriesToSend)
                {
                    Logger.GetLogger().DebugMessage("Sending entry. URL: " + currentURL + ", Title: " + pageTitle + ", Entry: " + entry);
                }
            }

            // send the data.
            dataExchangeAPIClient.AddEntries(entriesToSend);
        }

        /// <summary>
        /// Visible for testing.
        /// </summary>
        /// <param name="currentURL"></param>
        /// <param name="pageTitle"></param>
        /// <param name="conf"></param>
        /// <returns></returns>
        public static List<string> createPath(string currentURL, string pageTitle, EUEConfiguration conf)
        {
            List<string> path = new List<string>();

            path.Add(getScriptName(conf.ScriptName));
            path.Add(TimerBuilder.TimersName);

            string customName = conf.CurrentCustomName;
            if (customName != null)
            {
                List<string> pathItems = splitOnSlash(customName);
                path.AddRange(pathItems);

                // the custom name is only used once.
                conf.CurrentCustomName = null;
            }
            else if (PathNamingPolicy.URL == conf.PathNamingPolicy)
            {
                string prettyURL = getPrettyURL(currentURL, conf.RegexToCleanURLs);
                path.AddRange(splitOnSlash(prettyURL));

            }
            else if (PathNamingPolicy.ACTION == conf.PathNamingPolicy)
            {
                path.Add(conf.CurrentLastAction);

            }
            else if (PathNamingPolicy.TITLE == conf.PathNamingPolicy)
            {
                if (pageTitle == null)
                {
                    path.Add(pageTitle);
                }
                else
                {
                    path.AddRange(splitOnSlash(pageTitle));
                }
            }
            else
            {
                throw new WebDriverException("Unrecognized PathNamingPolicy");
            }
            return path;
        }

        private static string getScriptName(string scriptName)
        {
            if (scriptName != null && !scriptName.Equals(""))
            {
                return scriptName;
            }
            return ConfigurationHelper.getUnitTestName();
        }

        private static List<string> splitOnSlash(string customName)
        {
            string[] fragments = customName.Split('/');
            List<string> list = new List<string>();
            foreach (string fragment in fragments)
            {
                if (fragment != null && fragment != "")
                {
                    list.Add(fragment);
                }
            }
            return list;
        }

        private static string getPrettyURL(string urlStr, string regexURLCleaner)
        {
            string cleanedURL = urlStr;
            try
            {
                string urlStrTrimed = urlStr == null ? "" : urlStr.Trim();
                Uri url = new Uri(urlStrTrimed);


                string localPath = url.LocalPath;
                string host = url.Host;

                int index = urlStr.IndexOf(localPath, host.Length + "//".Length);
                cleanedURL = urlStr.Substring(index);
                
                
                cleanedURL = HttpUtility.UrlDecode(cleanedURL, Encoding.UTF8);
            }
            catch (Exception e) {
                // absorb exception
            }

            Match match = Regex.Match(cleanedURL, regexURLCleaner);

            if (match.Success)
            {
                StringBuilder newURL = new StringBuilder();
                for (int i = 1; i <= match.Groups.Count; i++)
                {
                    newURL.Append(Objects.FirstNonNull(match.Groups[i], ""));
                }
                cleanedURL = newURL.ToString();
            }
            return cleanedURL;
        }

        /// <summary>
        /// Throw the stored exception if there is one.
        /// </summary>
        void throwStoredException()
        {
            try
            {
                if (exception != null)
                {
                    throw exception;
                }
            }
            finally
            {
                exception = null;
            }
        }
    }
}
