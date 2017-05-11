namespace Alpha.BetLists.Criteria.Outstanding
{
    using System;
    using Fanex.Data.Repository;

    public class GetBetListRunningByDateCriteria : CriteriaBase
    {
        public GetBetListRunningByDateCriteria(
            int custId,
            string productIds,
            DateTime fromDate,
            DateTime toDate,
            bool getAll,
            string language,
            string userName)
        {
            CustId = custId;
            FromDate = fromDate;
            ToDate = toDate;
            GetAll = getAll;
            Language = language;
            UserName = userName;
            ProductIds = productIds;
        }

        public int CustId { get; }

        public DateTime FromDate { get; }

        public DateTime ToDate { get; }

        public bool GetAll { get; }

        public string Language { get; }

        public string UserName { get; }

        public string ProductIds { get; }

        public override string GetSettingKey() => "Age_BetList_SelRunningByDate_Mem";

        public override bool IsValid() =>
            CustId > 0
            && !string.IsNullOrEmpty(ProductIds)
            && ToDate >= FromDate;
    }
}