using System;
using OpenQA.Selenium;

namespace NeoLoadSelenium.neoload.config
{
    public enum Mode
    {
        DESIGN, END_USER_EXPERIENCE, NO_API
    }

    public class ModeHelper
    {
        public static string MODE_NO_API = "NoApi";
        public static string MODE_DESIGN = "Design";
        public static string MODE_END_USER_EXPERIENCE = "EndUserExperience";

        public static Mode getMode()
        {
            string modeString = ConfigurationHelper.getPropertyValue(ConfigurationHelper.OPT_SELENIUM_WRAPPER_MODE, MODE_NO_API);
            if (MODE_DESIGN.Equals(modeString, StringComparison.InvariantCultureIgnoreCase))
            {
                return Mode.DESIGN;
            }
            else if (MODE_END_USER_EXPERIENCE.Equals(modeString, StringComparison.InvariantCultureIgnoreCase))
            {
                return Mode.END_USER_EXPERIENCE;
            }
            else if (MODE_NO_API.Equals(modeString, StringComparison.InvariantCultureIgnoreCase))
            {
                return Mode.NO_API;
            }
            else
            {
                throw new WebDriverException("Unrecognized mode: " + modeString);
            }
        }
    }
}
