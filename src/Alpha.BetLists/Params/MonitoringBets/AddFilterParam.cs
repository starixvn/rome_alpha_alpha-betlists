namespace Alpha.BetLists.Params.MonitoringBets
{
    public class AddFilterParam : UpdateFilterParam
    {
        public AddFilterParam(
            int custId,
            int subAccId,
            string filterName,
            bool isDefault,
            string bettype,
            int stake,
            string sporttypeList,
            bool isLive,
            string inputList,
            string watchList,
            byte isWatchList,
            string languageKey,
            string ip) : base(custId, subAccId, 0, filterName, isDefault, languageKey, ip)
        {
            Bettype = bettype;
            Stake = stake;
            SporttypeList = sporttypeList;
            IsLive = isLive;
            InputList = inputList;
            WatchList = watchList;
            IsWatchList = isWatchList;
        }

        public string Bettype { get; }

        public int Stake { get; }

        public string SporttypeList { get; }

        public bool IsLive { get; }

        public string InputList { get; }

        public string WatchList { get; }

        public byte IsWatchList { get; }
    }
}
