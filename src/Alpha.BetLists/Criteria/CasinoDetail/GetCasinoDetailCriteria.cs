namespace Alpha.BetLists.Criteria.CasinoDetail
{
    using Fanex.Data;
    using Fanex.Data.Repository;

    public class GetCasinoDetailCriteria : CriteriaBase
    {
        public GetCasinoDetailCriteria(
            long refNo,
            int custId,
            byte transType,
            int productId,
            int pageIndex,
            int pageSize)
        {
            RefNo = refNo;
            CustId = custId;
            TransType = transType;
            ProductId = productId;
            PageIndex = pageIndex;
            PageSize = pageSize;
        }

        public long RefNo { get; }

        public int CustId { get; }

        public byte TransType { get; }

        [SpParam(Ignored = true)]
        public int ProductId { get; }

        public int PageIndex { get; }

        public int PageSize { get; }

        public int TotalRows { get; set; }

        public override string GetSettingKey()
            => "Acc_BetList_Detail_" + ProductId;

        public override bool IsValid()
            => RefNo > 0
            && CustId > 0
            && ProductId > 0;
    }
}
