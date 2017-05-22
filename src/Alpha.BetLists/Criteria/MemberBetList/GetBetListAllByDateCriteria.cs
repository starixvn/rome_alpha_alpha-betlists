namespace Alpha.BetLists.Criteria.MemberBetList
{
    using Fanex.Data.Repository;
    using System;

    public class GetBetListAllByDateCriteria : CriteriaBase
    {
        public GetBetListAllByDateCriteria(
            int custId,
            string productIds,
            DateTime fromDate,
            DateTime toDate,
            string language,
            string userName)
        {
            CustId = custId;
            FromDate = fromDate;
            ToDate = toDate;
            Language = language;
            UserName = userName;
            ProductIds = productIds;
        }

        public int CustId { get; }

        public DateTime FromDate { get; }

        public DateTime ToDate { get; }

        public string Language { get; }

        public string UserName { get; }

        public string ProductIds { get; }

        public override string GetSettingKey() => "Age_BetList_SelAllByDate_Mem";

        public override bool IsValid() =>
            CustId > 0
            && !string.IsNullOrWhiteSpace(ProductIds)
            && ToDate >= FromDate;
    }
}