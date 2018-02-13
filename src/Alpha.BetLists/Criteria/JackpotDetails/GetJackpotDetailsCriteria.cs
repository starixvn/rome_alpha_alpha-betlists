namespace Alpha.BetLists.Criteria.JackpotDetails
{
    using Fanex.Data.Repository;

    public class GetJackpotDetailsCriteria : CriteriaBase
    {
        public GetJackpotDetailsCriteria(
            long transId,
            int custId,
            int betType)
        {
            TransId = transId;
            CustId = custId;
            BetType = betType;
        }

        public long TransId { get; }

        public int CustId { get; }

        public int BetType { get; }

        public override string GetSettingKey() => "Acc_BetList_Jackpot_Detail";

        public override bool IsValid() => TransId > 0 && CustId > 0 && BetType > 0;
    }
}