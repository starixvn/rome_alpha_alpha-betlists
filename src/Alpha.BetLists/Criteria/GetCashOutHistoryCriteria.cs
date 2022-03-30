namespace Alpha.BetLists.Criteria
{
    using System;
    using Fanex.Data.Repository;

    public class GetCashOutHistoryCriteria : CriteriaBase
    {
        public GetCashOutHistoryCriteria(long transId, long custId, DateTime winlostDate, DateTime balanceUpDay)
        {
            TransId = transId;
            CustId = custId;
            WinlostDate = winlostDate;
            BalanceUpDay = balanceUpDay;
        }

        public long TransId { get;}

        public long CustId { get; }

        public DateTime WinlostDate { get; }

        public DateTime BalanceUpDay { get; }

        public override string GetSettingKey()
            => "Acc_BetList_CashOut_History";

        public override bool IsValid()
            => TransId > 0 && CustId > 0;
    }
}