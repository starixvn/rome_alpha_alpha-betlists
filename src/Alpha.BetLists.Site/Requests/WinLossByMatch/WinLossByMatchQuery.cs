namespace Alpha.BetLists.Site.Requests.WinLossByMatch
{
    using BetLists.Models;
    using MediatR;
    using ViewModels.WinLossByMatch;

    public class WinLossByMatchQuery : BaseQuery, IRequest<WinLossByMatchViewModel>
    {
        public WinLossByMatchQuery(IUser viewer) : base(viewer)
        {
            Language = viewer.LangKey;
        }

        public int BetType { get; set; }

        public int LeagueId { get; set; }

        public int MatchId { get; set; }

        public string Race { get; set; }

        public int LiveIndicator { get; set; }

        public byte CustLevelId { get; set; }

        public UserLevel CustLevel => Enumeration.FromValue<UserLevel>(CustLevelId);
    }
}