namespace Alpha.BetLists.Params
{
    public class RNGBetDetailParam
    {
        public RNGBetDetailParam(
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
    }
}
