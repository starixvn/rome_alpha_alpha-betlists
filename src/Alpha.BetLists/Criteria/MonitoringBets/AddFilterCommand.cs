namespace Alpha.BetLists.Criteria.MonitoringBets
{
    using Alpha.BetLists.Params.MonitoringBets;
    using Fanex.Data.Repository;

    public class AddFilterCommand : NonQueryCommand
    {
        public AddFilterCommand(AddFilterParam param)
        {
            CustId = param.CustId;
            SubAccId = param.SubAccId;
            FilterName = param.FilterName;
            IsDefault = param.IsDefault;
            Bettype = param.Bettype;
            Stake = param.Stake;
            SporttypeList = param.SporttypeList;
            IsLive = param.IsLive;
            InputList = param.InputList;
            WatchList = param.WatchList;
            IsWatchList = param.IsWatchList;
            Language = param.LanguageKey;
            IP = param.IP;
        }

        public int CustId { get; }

        public int SubAccId { get; }

        public string FilterName { get; }

        public bool IsDefault { get; }

        public string Bettype { get; }

        public int Stake { get; }

        public string SporttypeList { get; }

        public bool IsLive { get; }

        public string InputList { get; }

        public string WatchList { get; }

        public byte IsWatchList { get; }

        public string Language { get; }

        public string IP { get; }

        public int ErrCode { get; set; }

        public string ErrMsg { get; set; }

        public override string GetSettingKey()
            => "Age_CustLastBetFilter_Ins";

        public override bool IsValid()
        {
            return CustId > 0
                && SubAccId >= 0
                && !string.IsNullOrWhiteSpace(FilterName)
                && !string.IsNullOrWhiteSpace(Bettype)
                && Stake >= 0
                && !string.IsNullOrWhiteSpace(SporttypeList);
        }
    }
}
