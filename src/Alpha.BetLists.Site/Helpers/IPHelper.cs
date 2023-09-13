namespace Alpha.BetLists.Site.Helpers
{
    using Sunplus.Agent.Model.Common;

    public static class IPHelper
    {
        public static string CustomerIP()
        {
            return AgentIPFormat.GetClientIP();
        }
    }
}