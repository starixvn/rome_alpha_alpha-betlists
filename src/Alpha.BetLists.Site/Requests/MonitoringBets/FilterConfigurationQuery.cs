using System.Web.Mvc;

namespace Alpha.BetLists.Site.Requests.MonitoringBets
{
    public class FilterConfigurationQuery
    {
        [AllowHtml]
        public string FilterName { get; set; }

        public int LimitRowCount { get; set; } = 100;

        public bool AutoStart { get; set; } = true;

        public bool Welcome { get; set; }
    }
}