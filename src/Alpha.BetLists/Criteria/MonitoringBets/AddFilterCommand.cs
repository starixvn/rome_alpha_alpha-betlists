namespace Alpha.BetLists.Criteria.MonitoringBets
{
    using System.Text;
    using Alpha.BetLists.Models.MonitoringBets;
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
            BetType = param.BetType;
            Stake = param.Stake;
            SportTypeList = ToXml(param.Sports);
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

        public string BetType { get; }

        public int Stake { get; }

        public string SportTypeList { get; }

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
                && !string.IsNullOrWhiteSpace(BetType)
                && Stake >= 0
                && !string.IsNullOrWhiteSpace(SportTypeList);
        }

        private static string ToXml(SportXml[] sports)
        {
            var xml = new StringBuilder("<SportList>");

            foreach (var item in sports)
            {
                AppendSportXml(xml, item);
            }

            xml.Append("</SportList>");

            return xml.ToString();
        }

        private static void AppendSportXml(StringBuilder xml, SportXml sport)
            => xml
            .Append("<Sport s=\"")
            .Append(sport.Sport)
            .Append("\" l=\"")
            .Append(sport.League)
            .Append("\" m=\"")
            .Append(sport.Match)
            .Append("\" h=\"")
            .Append(sport.Home)
            .Append("\" a=\"")
            .Append(sport.Away)
            .Append("\" iN=\"")
            .Append(sport.IsNeutral ? 1 : 0)
            .Append("\"/>");
    }
}
