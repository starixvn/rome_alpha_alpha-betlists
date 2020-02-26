namespace Alpha.BetLists.Criteria.LastBets
{
    using Constants;
    using Fanex.Data;
    using Fanex.Data.Repository;

    public class GetLastBetsCriteria : CriteriaBase
    {
        public GetLastBetsCriteria(
            int betType,
            int custId,
            long transId,
            int pageSize)
        {
            BetType = betType;
            CustId = custId;
            TransId = transId;
            PageSize = pageSize;
        }

        [SpParam(Ignored = true)]
        public int BetType { get; }

        public int CustId { get; }

        public long TransId { get; }

        [SpParam(SqlParamName = "limitrowcount")]
        public int PageSize { get; }

        public override string GetSettingKey()
        {
            switch (BetType)
            {
                case BetTypeIdConstants.AllBetTypes:
                    return "Ost_LastBets_All";
                case BetTypeIdConstants.Handicap:
                    return "Ost_LastBets_HDP";
                case BetTypeIdConstants.OverUnder:
                    return "Ost_LastBets_OU";
                default:
                    return "Ost_LastBets_Other";
            }
        }

        public override bool IsValid() => CustId > 0 && PageSize > 0;
    }
}
