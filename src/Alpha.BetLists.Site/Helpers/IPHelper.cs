namespace Alpha.BetLists.Site.Helpers
{
    using System.Web;
    using Configurations;

    public static class IPHelper
    {
        public static string CustomerIP()
        {
            var ipAddress = HttpContext.Current?.Request?.ServerVariables["HTTP_X_FORWARDED_FOR"]
                ?? HttpContext.Current?.Request?.ServerVariables["REMOTE_ADDR"];

            return (string.IsNullOrEmpty(ipAddress) || ipAddress.Equals(AppSettings.DefaultIpAddress))
                ? AppSettings.LocalIpAddress : ipAddress.Split(',')[0];
        }
    }
}