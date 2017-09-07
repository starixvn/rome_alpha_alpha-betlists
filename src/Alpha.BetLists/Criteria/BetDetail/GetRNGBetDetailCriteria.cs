namespace Alpha.BetLists.Criteria.BetDetail
{
    using Fanex.Data.Repository;

    public class GetRNGBetDetailCriteria : CriteriaBase
    {
        public GetRNGBetDetailCriteria(
            long refNo,
            int custId,
            byte transType,
            int pageIndex,
            int pageSize)
        {
            RefNo = refNo;
            CustId = custId;
            TransType = transType;
            PageIndex = pageIndex;
            PageSize = pageSize;
        }

        public long RefNo { get; }

        public int CustId { get; }

        public byte TransType { get; }

        public int PageIndex { get; }

        public int PageSize { get; }

        public int TotalRows { get; set; }

        public override string GetSettingKey()
            => "Acc_BetList_RNG_Detail";

        public override bool IsValid()
            => RefNo > 0 && CustId > 0;
    }
}
