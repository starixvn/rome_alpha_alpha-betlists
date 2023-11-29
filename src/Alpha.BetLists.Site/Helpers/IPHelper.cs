namespace Alpha.BetLists.Site.Helpers
{
    public static class IPHelper
    {
        public static string CustomerIP()
        {
            return Web.Helpers.IPHelper.GetClientIP();
        }
    }
}