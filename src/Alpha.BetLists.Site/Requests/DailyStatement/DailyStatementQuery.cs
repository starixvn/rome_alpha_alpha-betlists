namespace Alpha.BetLists.Site.Requests.DailyStatement
{
    using System;
    using BetLists.Models;
    using MediatR;
    using ViewModels.DailyStatement;

    public class DailyStatementQuery : BaseQuery, IRequest<DailyStatementViewModel>
    {
        public DailyStatementQuery(IUser viewer) : base(viewer)
        {
        }

        public DateTime WinLossDate { get; set; }

        public int TransType { get; set; }

        public bool SupportRiskAmount => TransType == 3;
    }
}
