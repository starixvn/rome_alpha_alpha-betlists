namespace Alpha.BetLists.Params.MonitoringBets
{
    using Alpha.BetLists.Models.MonitoringBets;

    public class AddFilterParam : UpdateFilterParam
    {
        public AddFilterParam(
            int custId,
            int subAccId,
            string filterName,
            bool isDefault,
            string betType,
            int stake,
            SportXml[] sports,
            byte isLive,
            string inputList,
            string myWatchlist,
            byte filterByMembers,
            string languageKey,
            string ip) : base(custId, subAccId, 0, filterName, isDefault, languageKey, ip)
        {
            BetType = betType;
            Stake = stake;
            Sports = sports;
            IsLive = isLive;
            InputList = inputList;
            MyWatchlist = myWatchlist;
            FilterByMembers = filterByMembers;
        }

        public string BetType { get; }

        public int Stake { get; }

        public SportXml[] Sports { get; }

        public byte IsLive { get; }

        public byte FilterByMembers { get; }

        public string InputList { get; }

        public string MyWatchlist { get; }
    }
}
