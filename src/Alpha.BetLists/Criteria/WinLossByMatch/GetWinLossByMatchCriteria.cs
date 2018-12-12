namespace Alpha.BetLists.Criteria.WinLossByMatch
{
    using Fanex.Data;
    using Fanex.Data.Repository;
    using Models;

    public class GetWinLossByMatchCriteria : CriteriaBase
    {
        public GetWinLossByMatchCriteria(
             int custId,
             int matchId,
             int betType,
             string custName,
             bool liveIndicator,             
             UserLevel custLevel)
        {
            CustId = custId;
            MatchId = matchId;
            BetType = betType;

            CustName = custName;
            LiveIndicator = liveIndicator;
            RoleId = custLevel.Value;
        }

        public int CustId { get; }

        public int MatchId { get; }

        public int BetType { get; }

        [SpParam(SqlParamName = "Username")]
        public string CustName { get; }

        public bool LiveIndicator { get; }

        public int RoleId { get; }

        public override string GetSettingKey() => "Age_BetList_SelWinlossByBettypeMatch";          

        public override bool IsValid() => CustId > 0;
    }
}
