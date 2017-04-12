namespace Alpha.BetLists.Site.Helpers
{
    using System.Collections.Generic;
    using System.Text.RegularExpressions;
    using System.Web;
    using BetLists.Models;

    public static class IPHelper
    {
        public static string CustomerIP()
        {
            var ipAddress = HttpContext.Current?.Request?.ServerVariables["HTTP_X_FORWARDED_FOR"]
                ?? HttpContext.Current?.Request?.ServerVariables["REMOTE_ADDR"];

            return (string.IsNullOrEmpty(ipAddress) || ipAddress.Equals("::1")) ? "127.0.0.1" : ipAddress.Split(',')[0];
        }
    }
}