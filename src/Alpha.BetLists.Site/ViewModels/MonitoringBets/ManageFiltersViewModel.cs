namespace Alpha.BetLists.Site.ViewModels.MonitoringBets
{
    using System.Collections.Generic;
    using Alpha.BetLists.Models.MonitoringBets;
    using Alpha.BetLists.Site.Requests.MonitoringBets;

    public class ManageFiltersViewModel
    {
        public ManageFiltersViewModel(
            IEnumerable<FilterConfiguration> filterConfiguration,
            ManageFiltersQuery message)
        {
            FilterConfiguration = filterConfiguration;
            FilterName = message.FilterName;
            LimitRowCount = message.LimitRowCount;
            AutoStart = message.AutoStart;
        }

        public IEnumerable<FilterConfiguration> FilterConfiguration { get; }

        public string FilterName { get; }

        public int LimitRowCount { get; }

        public bool AutoStart { get; }
    }
}