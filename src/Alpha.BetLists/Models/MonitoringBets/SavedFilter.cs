namespace Alpha.BetLists.Models.MonitoringBets
{
    public class SavedFilter
    {
        public SavedFilter(
            int filterId,
            string filterName,
            bool isDefault,
            string betTypes,
            int stake,
            string members)
        {
            FilterId = filterId;
            FilterName = filterName;
            IsDefault = isDefault;
            BetTypes = betTypes;
            Stake = stake;
            Members = members;
        }

        public int FilterId { get; }

        public string FilterName { get; }

        public bool IsDefault { get; }

        public string BetTypes { get; }

        public int Stake { get; }

        public string Members { get; }
    }
}
