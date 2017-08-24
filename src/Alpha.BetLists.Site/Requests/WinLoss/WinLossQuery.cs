namespace Alpha.BetLists.Site.Requests.WinLoss
{
    using System;
    using BetLists.Models;
    using Extensions;
    using MediatR;
    using Utils;
    using ViewModels.WinLoss;

    public class WinLossQuery : BaseQuery, IRequest<WinLossViewModel>
    {
        public WinLossQuery(IUser viewer) : base(viewer)
        {
        }

        public bool IsGroupHeadline { get; set; }

        public DateTime MinDate => IsGroupHeadline
            ? SystemTime.Today.AddMonths(-3).FirstDayOfMonth()
            : DateTime.MinValue;
    }
}