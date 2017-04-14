namespace Alpha.BetLists.Criteria.Parlay
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using Fanex.Data.Repository;

    public class GetSystemParlayDetailCriteria : CriteriaBase
    {
        public GetSystemParlayDetailCriteria(
            long transId,
            long refNo,
            long betId,
            int custId,
            DateTime balanceUpDate,
            DateTime moveBetDate,
            string language)
        {
            TransId = transId;
            Language = language;
            RefNo = refNo;
            BetId = betId;
            CustId = custId;
            BalanceUpDay = balanceUpDate;
            MoveBetDay = moveBetDate;
        }

        public long TransId { get; }

        public long RefNo { get; }

        public long BetId { get; }

        public int CustId { get; }

        public DateTime BalanceUpDay { get; }

        public DateTime MoveBetDay { get; }

        public string Language { get; }

        public override string GetSettingKey() => "Acc_BetlistCombinationParlay_Details";

        public override bool IsValid() => TransId > 0 && !string.IsNullOrWhiteSpace(Language);
    }
}