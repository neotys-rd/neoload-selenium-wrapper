﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NeoLoadSelenium.neoload.config
{
    public class CommonConfiguration
    {
        public string ApiKey { get; }
        
        public CommonConfiguration(string apiKey)
        {
            this.ApiKey = apiKey;
        }
    }

    public class EUEConfiguration : CommonConfiguration
    {
        public string DataExchangeAPIUrl { get; }
        public bool IsDebugEnabled { get; }
        public string Location { get; }
        public string RegexToCleanURLs { get; }
        public string Software { get; }
        public string Os { get; }
        public string Hardware { get; }
        public string InstanceId { get; }
        public string ScriptName { get; }
        public bool NavigationTimingEnabled { get; }
        public PathNamingPolicy PathNamingPolicy { get; }

        public string CurrentCustomName {get; set;}
        public string CurrentLastAction { get; set; }

        public EUEConfiguration(
            string apiKey, 
            string dataExchangeAPIUrl, 
            bool isDebugEnabled, 
            string location, 
            string regexToCleanURLs,
            string software,
            string os,
            string hardware,
            string instanceId,
            string scriptName,
            PathNamingPolicy pathNamingPolicy,
            bool navigationTimingEnabled
            ) :base(apiKey)
        {
            this.DataExchangeAPIUrl = dataExchangeAPIUrl;
            this.IsDebugEnabled = isDebugEnabled;
            this.Location = location;
            this.RegexToCleanURLs = regexToCleanURLs;
            this.Software = software;
            this.Os = os;
            this.Hardware = hardware;
            this.InstanceId = instanceId;
            this.ScriptName = scriptName;
            this.PathNamingPolicy = pathNamingPolicy;
            this.NavigationTimingEnabled = navigationTimingEnabled;
            this.CurrentCustomName = null;
            this.CurrentLastAction = "(none)";
        }
    }

    public class DesignConfiguration : CommonConfiguration
    {
        public string DesignAPIUrl { get; }
        public string UserPathName { get; }
        public string ProjectPath { get; }
        
        public DesignConfiguration(string apiKey, string designAPIUrl, string userPathName, string projectPath) : base(apiKey)
        {
            this.DesignAPIUrl = designAPIUrl;
            this.UserPathName = userPathName;
            this.ProjectPath = projectPath;
        }
    }

    public enum PathNamingPolicy
    {
        /// <summary>
        /// Parse the URL to create a path.
        /// </summary>
        URL,
        /// <summary>
        /// Use the title.
        /// </summary>
        TITLE,
        /// <summary>
        /// Use a string representing the most recent action that was done.
        /// </summary>
        ACTION
    }
}
