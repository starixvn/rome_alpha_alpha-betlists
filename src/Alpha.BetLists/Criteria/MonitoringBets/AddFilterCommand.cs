namespace Alpha.BetLists.Criteria.MonitoringBets
{
    using Fanex.Data.Repository;

    public class AddFilterCommand : NonQueryCommand
    {
        public AddFilterCommand(
            int custId,
            int subAccId,
            string filterName,
            bool isDefault,
            string betType,
            int stake,
            string sportTypeList,
            byte liveStatus,
            byte filterByMembers,
            string inputList,
            string myWatchlist,
            string language,
            string ip,
            bool isNotify)
        {
            CustId = custId;
            SubAccId = subAccId;
            FilterName = filterName;
            IsDefault = isDefault;
            BetType = betType;
            Stake = stake;
            SportTypeList = sportTypeList;
            IsLive = liveStatus;
            IsWatchList = filterByMembers;
            InputList = inputList;
            WatchList = myWatchlist;
            Language = language;
            IP = ip;
            IsNotify = isNotify;
        }

        public int CustId { get; }

        public int SubAccId { get; }

        public string FilterName { get; }

        public bool IsDefault { get; }

        public bool IsNotify { get; }

        public string BetType { get; }

        public int Stake { get; }

        public string SportTypeList { get; }

        public byte IsLive { get; }

        public byte IsWatchList { get; }

        public string InputList { get; }

        public string WatchList { get; }

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
                && !string.IsNullOrWhiteSpace(BetType)
                && Stake >= 0
                && !string.IsNullOrWhiteSpace(SportTypeList);
        }
    }
}
