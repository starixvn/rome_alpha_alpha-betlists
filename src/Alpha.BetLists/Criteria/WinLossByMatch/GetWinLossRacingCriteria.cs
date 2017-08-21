namespace Alpha.BetLists.Criteria.WinLossByMatch
{
    using System;
    using Fanex.Data;
    using Fanex.Data.Repository;
    using Models;

    public class GetWinLossRacingCriteria : CriteriaBase
    {
        public GetWinLossRacingCriteria(
            int custId,
            int leagueId,
            int betType,
            string race,
            string language,
            DateTime fromDate,
            DateTime toDate,
            DateTime moveBetsDate,
            UserLevel custLevel)
        {
            CustId = custId;
            LeagueId = leagueId;
            BetType = betType;
            Race = race;

            Language = language;
            FromDate = fromDate;
            ToDate = toDate;
            MoveBetsDate = moveBetsDate;
            CustLevel = custLevel;
        }

        public int CustId { get; }

        public int LeagueId { get; }

        public int BetType { get; }

        public string Race { get; }

        public string Language { get; }

        public DateTime FromDate { get; }

        public DateTime ToDate { get; }

        [SpParam(SqlParamName = "MoveBetsDay")]
        public DateTime MoveBetsDate { get; }

        [SpParam(Ignored = true)]
        public UserLevel CustLevel { get; }

        public override string GetSettingKey() =>
            CustLevel.IsSuperLevel()
            ? "Age_BetList_SelWinlossHR_SAg"
            : CustLevel.IsMasterLevel()
            ? "Age_BetList_SelWinlossHR_MAg"
            : "Age_BetList_SelWinlossHR_Ag";

        public override bool IsValid() =>
            CustId > 0
            && ToDate >= FromDate;
    }
}
