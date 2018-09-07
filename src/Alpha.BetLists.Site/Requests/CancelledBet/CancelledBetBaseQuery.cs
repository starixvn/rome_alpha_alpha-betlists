namespace Alpha.BetLists.Site.Requests.CancelledBet
{
    using System;
    using Alpha.BetLists.Models;
    using Alpha.BetLists.Site.ViewModels.CancelledBet;
    using BetLists.Constants;
    using Extensions;
    using MediatR;
    using Services;

    public class CancelledBetBaseQuery : BaseQuery, IRequest<CancelledBetViewModel>
    {
        protected CancelledBetBaseQuery(IUser viewer) : base(viewer)
        {
            MinDate = ReportDateServiceImpl.Instance.GetBalanceUpDay();
            FromDate = FromDate == DateTime.MinValue ? DateTime.Today : FromDate;
            ToDate = ToDate == DateTime.MinValue ? DateTime.Today : ToDate;
            StatusIds = string.IsNullOrEmpty(StatusIds) ? TicketStatusIdConstants.All.JoinWithComma() : StatusIds;
        }

        public string StatusIds { get; set; }

        public DateTime MinDate { get; }

        public int UserSelectedDateRangeOption { get; set; }

        public int PageIndex { get; set; }

        public int PageSize { get; set; }
    }
}