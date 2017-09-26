namespace Alpha.BetLists.Site.Requests.WinLossByMatch
{
    using BetLists.Models;
    using MediatR;
    using ViewModels.ParlayWinLossByMatch;

    public class ParlayWinLossByMatchQuery : BaseQuery, IRequest<ParlayWinLossByMatchViewModel>
    {
        public ParlayWinLossByMatchQuery(IUser viewer) : base(viewer)
        {
            Language = viewer.LangKey;
        }

        public int BetType { get; set; }

        public byte CustLevelId { get; set; }

        public UserLevel CustLevel => Enumeration.FromValue<UserLevel>(CustLevelId);
    }
}