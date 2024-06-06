namespace Alpha.BetLists.Criteria.MemberBetList
{
    using System;
    using AlphaPlus.Configurations;
    using Fanex.Data.Repository;

    public class GetBetListAllByDateCriteria : CriteriaBase
    {
        public GetBetListAllByDateCriteria(
            int custId,
            string productIds,
            DateTime fromDate,
            DateTime toDate,
            string userName)
        {
            CustId = custId;
            FromDate = fromDate;
            ToDate = toDate;
            UserName = userName;
            ProductIds = productIds;
            Is789y = AppSettings.IsSiteLabel777;
        }

        public int CustId { get; }

        public DateTime FromDate { get; }

        public DateTime ToDate { get; }

        public string UserName { get; }

        public string ProductIds { get; }

        public bool Is789y { get; }

        public override string GetSettingKey() => "Age_BetList_SelAllByDate_Mem";

        public override bool IsValid() =>
            CustId > 0
            && !string.IsNullOrWhiteSpace(ProductIds)
            && ToDate >= FromDate
            && FromDate > DateTime.MinValue;
    }
}