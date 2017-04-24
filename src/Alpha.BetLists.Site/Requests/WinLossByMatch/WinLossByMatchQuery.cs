using Alpha.BetLists.Models;
using Alpha.BetLists.Site.ViewModels.WinLoss;
using MediatR;

namespace Alpha.BetLists.Site.Requests.WinLossByMatch
{
    public class WinLossByMatchQuery : BaseQuery, IRequest<WinLossViewModel>
    {
        public WinLossByMatchQuery(IUser viewer) : base(viewer)
        {
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