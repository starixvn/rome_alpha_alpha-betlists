using System;
using Alpha.BetLists.Models;
using Fanex.Data;
using Fanex.Data.Repository;

namespace Alpha.BetLists.Criteria.WinLossByMatch
{
    public class GetWinLossOutrightCriteria : CriteriaBase
    {
        public GetWinLossOutrightCriteria(
            int custId,
            int leagueId,
            string language,
            DateTime fromDate,
            DateTime toDate,
            DateTime moveBetsDate,
            UserLevel custLevel)
        {
            CustId = custId;
            LeagueId = leagueId;
            Language = language;
            FromDate = fromDate;
            ToDate = toDate;
            MoveBetsDate = moveBetsDate;
            CustLevel = custLevel;
        }

        public int CustId { get; }

        public int LeagueId { get; }

        public string Language { get; }

        public DateTime FromDate { get; }

        public DateTime ToDate { get; }

        public DateTime MoveBetsDate { get; }

        [SpParam(Ignored = true)]
        public UserLevel CustLevel { get; }

        public override string GetSettingKey() =>
            CustLevel.IsSuperLevel()
            ? "Age_BetList_SelWinlossOutright_SAg"
            : CustLevel.IsMasterLevel()
            ? "Age_BetList_SelWinlossOutright_MAg"
            : "Age_BetList_SelWinlossOutright_Ag";

        public override bool IsValid() =>
            CustId > 0
            && ToDate >= FromDate;
    }
}
