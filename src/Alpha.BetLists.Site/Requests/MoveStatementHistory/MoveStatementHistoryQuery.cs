namespace Alpha.BetLists.Site.Requests.MoveStatementHistory
{
    using Alpha.BetLists.Models;
    using Alpha.BetLists.Site.ViewModels.MoveStatementHistory;
    using MediatR;

    public class MoveStatementHistoryQuery : BaseQuery, IRequest<MoveStatementHistoryViewModel>
    {
        public MoveStatementHistoryQuery(IUser viewer) : base(viewer)
        {
        }

        public long TransId { get; set; }
    }
}