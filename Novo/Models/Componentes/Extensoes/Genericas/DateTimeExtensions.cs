namespace WEBAPI_VOPAK
{

    using System;

    public static class DateTimeExtensions
    {
        public static string ToLogTimeLine(this DateTime dt)
        {
            string pattern = "{0} ";
            return string.Format(pattern, dt.ToString());
        }

        public static string ToFilePrefix(this DateTime dt)
        {
            string pattern = "{0}{1}{2}";
            return string.Format(pattern, dt.Year, dt.Month, dt.Day);
        }
    }
}