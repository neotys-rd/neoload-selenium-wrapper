using Castle.DynamicProxy;
using NeoLoadSelenium.neoload.config;
using OpenQA.Selenium;
using System;

namespace NeoLoadSelenium.neoload.interceptor.eue_element
{
    public class EUEElementInterceptor : IInterceptor
    {
        private IWebDriver driver;
        private EUEEntryHandler dataExchangeEntryHelper;
        private EUEConfiguration conf;
        private EUEInterceptedMethods methods;

        public EUEElementInterceptor(IWebDriver driver, EUEEntryHandler dataExchangeEntryHelper, EUEConfiguration conf, EUEInterceptedMethods methods)
        {
            this.driver = driver;
            this.dataExchangeEntryHelper = dataExchangeEntryHelper;
            this.conf = conf;
            this.methods = methods;
        }

        public void Intercept(IInvocation invocation)
        {
            if(isSetLast(invocation))
            {
                DoOnMethodsRequiringToSetLastAction(invocation.Method.Name, invocation.Arguments);
            }
            
            dataExchangeEntryHelper.ResetStartTime();

            try
            {
                invocation.Proceed();

                object returnedValue = invocation.ReturnValue;
                invocation.ReturnValue = EUEElementProxifier.proxifyIfRequired(returnedValue, driver, dataExchangeEntryHelper, conf);
            } catch(Exception e)
            {
                if(isOnException(invocation) || isAlwaysSent(invocation))
                {
                    dataExchangeEntryHelper.SendEntryThrow(WebDrivers.GetURL(driver), WebDrivers.GetTitle(driver), wrapIfRequired(e), invocation.Method.Name, WebDrivers.GetAdvancedValues(driver, conf));
                }
                throw e;
            }

            if(isAlwaysSent(invocation)) {
                dataExchangeEntryHelper.SendEntry(WebDrivers.GetURL(driver), WebDrivers.GetTitle(driver), invocation.Method.Name, WebDrivers.GetAdvancedValues(driver, conf));
            }
        }

        private static WebDriverException wrapIfRequired(Exception e)
        {
            if(e is WebDriverException)
            {
                return e as WebDriverException;
            }
            return new WebDriverException("Exception from EUE element interceptor", e);
        }

        private void DoOnMethodsRequiringToSetLastAction(string methodName, object argument)
        {
            this.conf.CurrentLastAction = methodName + " " + argument;
        }
        
        private bool isSetLast(IInvocation invocation)
        {
            return methods.setLast.Contains(invocation.Method.Name);
        }

        private bool isAlwaysSent(IInvocation invocation)
        {
            return methods.alwaysSend.Contains(invocation.Method.Name);
        }

        private bool isOnException(IInvocation invocation)
        {
            return methods.onException.Contains(invocation.Method.Name);
        }
    }
}
