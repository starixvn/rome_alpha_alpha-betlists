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
                    return "Age_Monitor_BetlistSel_Ag_AllBettypes";
                case BetTypeIdConstants.Handicap:
                    return "Age_Monitor_BetlistSel_Ag_HDP";
                case BetTypeIdConstants.OverUnder:
                    return "Age_Monitor_BetlistSel_Ag_OU";
                default:
                    return "Age_Monitor_BetlistSel_Ag_OtherBettypes";
            }
        }

        public override bool IsValid() => CustId > 0 && PageSize > 0;
    }
}
