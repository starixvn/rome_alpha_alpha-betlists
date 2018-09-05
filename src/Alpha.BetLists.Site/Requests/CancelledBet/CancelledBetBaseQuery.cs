namespace Alpha.BetLists.Site.Requests.CancelledBet
{
    using System;
    using Alpha.BetLists.Models;
    using Alpha.BetLists.Site.ViewModels.CancelledBet;
    using Extensions;
    using MediatR;
    using Services;
    using Utils;

    public class CancelledBetBaseQuery : BaseQuery, IRequest<CancelledBetViewModel>
    {
        protected CancelledBetBaseQuery(IUser viewer) : base(viewer)
        {
            MinDate = ReportDateServiceImpl.Instance.GetBalanceUpDay();
            FromDate = FromDate == DateTime.MinValue ? DateTime.Now : FromDate;
            ToDate = ToDate == DateTime.MinValue ? DateTime.Now : ToDate;            
        }

        public string StatusIds { get; set; }

        public DateTime MinDate { get; }

        public int UserSelectedDateRangeOption { get; set; }

        public int PageIndex { get; set; }

        public int PageSize { get; set; }
    }
}