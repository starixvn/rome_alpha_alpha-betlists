namespace Alpha.BetLists.Criteria.ProgressiveJackpot
{
    using System;
    using Fanex.Data;
    using Fanex.Data.Repository;

    public class GetJackpotDetailsCriteria : CriteriaBase
    {
        public GetJackpotDetailsCriteria(
            long transId,
            int custId,
            int betType,
            DateTime moveBetsDate,
            DateTime balanceUpDate)
        {
            TransId = transId;
            CustId = custId;
            BetType = betType;
            MoveBetsDate = moveBetsDate;
            BalanceUpDate = balanceUpDate;
        }

        public long TransId { get; }

        public int CustId { get; }

        public int BetType { get; }

        [SpParam(SqlParamName = "movebetsday")]
        public DateTime MoveBetsDate { get; }

        [SpParam(SqlParamName = "BalanceUpDay")]
        public DateTime BalanceUpDate { get; }

        public override string GetSettingKey() => "Acc_BetList_Jackpot_Detail";

        public override bool IsValid() => TransId > 0 && CustId > 0 && BetType > 0;
    }
}