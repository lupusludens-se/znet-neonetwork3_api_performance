using TimeZoneConverter;

namespace SE.Neo.Common.Service.Interfaces
{
    public class TimeZoneHelper
    {
        /// <summary>
        /// Tries to return an IANA TimeZoneInfo object by converting the WindowsName. If IANA lookup fails it will attempt to return the windows TimeZoneInfo object.
        /// </summary>
        /// <param name="windowsName">Windows timezone name</param>
        /// <returns>TimeZoneInfo object cooresponding to the IANA timezone which matches the Windows timezone</returns>
        /// <exception cref="TimeZoneNotFoundException"></exception>
        protected TimeZoneInfo GetTimeZoneInfoByWindowsName(string windowsName)
        {
            if (TZConvert.TryWindowsToIana(windowsName, out string ianaName))
            {
                if (TZConvert.TryGetTimeZoneInfo(ianaName, out TimeZoneInfo timeZoneInfo))
                {
                    return timeZoneInfo;
                }
            }
            else
            {
                if (TZConvert.TryGetTimeZoneInfo(windowsName, out TimeZoneInfo timeZoneInfo))
                {
                    return timeZoneInfo;
                }
            }

            try
            {
                // try to find timezone info using default .NET (>= 6.0) mechanism
                return TimeZoneInfo.FindSystemTimeZoneById(windowsName);
            }
            catch (Exception ex)
            {
                throw new TimeZoneNotFoundException($"Could not convert {windowsName} to IANA or retrieve Windows timezone.", ex);
            }
        }
    }
}

