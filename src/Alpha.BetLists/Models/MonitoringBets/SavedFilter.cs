namespace Alpha.BetLists.Models.MonitoringBets
{
    public class SavedFilter
    {
        public SavedFilter()
        {
            FilterId = -1;
            FilterName = AppResources.Resources.PleaseSelect;
        }

        public SavedFilter(
            int filterId,
            string filterName,
            bool isDefault,
            string betTypes,
            int stake,
            byte eventStatus,
            string sportTypes,
            string leagues,
            string members,
            byte filterByMembers)
        {
            FilterId = filterId;
            FilterName = filterName;
            IsDefault = isDefault;
            BetTypes = betTypes;
            Stake = stake;
            EventStatus = eventStatus;
            SportTypes = sportTypes;
            Leagues = leagues;
            Members = members;
            FilterByMembers = filterByMembers;
        }

        public int FilterId { get; }

        public string FilterName { get; }

        public bool IsDefault { get; }

        public string BetTypes { get; }

        public int Stake { get; }

        public byte EventStatus { get; }

        public string SportTypes { get; }

        public string Leagues { get; }

        public string Members { get; }

        public byte FilterByMembers { get; }
    }
}
