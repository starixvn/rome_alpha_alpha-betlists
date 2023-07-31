namespace Alpha.BetLists.Params.MonitoringBets
{
    using System;
    using Alpha.BetLists.Models.MonitoringBets;

    [Serializable]
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
            byte liveStatus,
            string inputList,
            string inputListMembers,
            string myWatchlist,
            string myWatchlistMembers,
            byte filterByMembers,
            string languageKey,
            string ip) : base(custId, subAccId, 0, filterName, isDefault, languageKey, ip)
        {
            BetType = betType;
            Stake = stake;
            Sports = sports;
            LiveStatus = liveStatus;
            InputList = inputList;
            InputListMembers = inputListMembers;
            MyWatchlist = myWatchlist;
            MyWatchlistMembers = myWatchlistMembers;
            FilterByMembers = filterByMembers;
        }

        public string BetType { get; }

        public int Stake { get; }

        public SportXml[] Sports { get; }

        public byte LiveStatus { get; }

        public byte FilterByMembers { get; }

        public string InputList { get; }

        public string InputListMembers { get; }

        public string MyWatchlist { get; }

        public string MyWatchlistMembers { get; }
    }
}
