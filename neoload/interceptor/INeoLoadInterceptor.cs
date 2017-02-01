using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NeoLoadSelenium.neoload.interceptor
{
    public interface INeoLoadInterceptor
    {
        void DoOnStart();

        string DoOnGetRegexToCleanURLs();

        void DoOnSetCustomName(string customName);

        void DoOnFindOneOrMoreElements(string methodName, By by);

        void DoOnFindOneOrMoreElementsWithException(string findElementTypeMethodName, By by, WebDriverException e);
        
        void BeforeSetUrl();

        void AfterSetUrl();

        void DoOnSetUrlWithException(WebDriverException e, string url);

        void DoOnStartTransaction(string name);

        void DoOnStopTransaction();

        void DoOnQuit();

        object proxify(object original);
    }
}
