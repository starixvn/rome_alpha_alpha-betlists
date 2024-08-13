namespace Alpha.BetLists.Criteria.MonitoringBets
{
    using Fanex.Data.Repository;

    public class DeleteFilterCommand : NonQueryCommand
    {
        public DeleteFilterCommand(
            int custId,
            int subAccId,
            int filterId,
            string language,
            string ip)
        {
            CustId = custId;
            SubAccId = subAccId;
            FilterId = filterId;
            Language = language;
            IP = ip;
        }

        public int CustId { get; }

        public int SubAccId { get; }

        public int FilterId { get; }

        public string Language { get; }

        public string IP { get; }

        public int ErrCode { get; set; }

        public string ErrMsg { get; set; }

        public override string GetSettingKey()
            => "Age_CustLastBetFilter_Del";

        public override bool IsValid()
            => CustId > 0 && SubAccId >= 0 && FilterId > 0;
    }
}
