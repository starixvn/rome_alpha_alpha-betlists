namespace Alpha.BetLists.Site.Requests.BetDetail
{
    using BetLists.Models;

    public class BetDetailQuery : BaseQuery
    {
        public BetDetailQuery(IUser viewer) : base(viewer)
        {
            PageIndex = 1;
            PageSize = 100;
        }

        public long RefNo { get; set; }

        public byte TransType { get; set; }

        public int PageIndex { get; set; }

        public int PageSize { get; set; }
    }
}
