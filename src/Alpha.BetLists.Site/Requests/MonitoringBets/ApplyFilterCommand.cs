namespace Alpha.BetLists.Site.Requests.MonitoringBets
{
    using Alpha.BetLists.Models;
    using Alpha.Web.Models;
    using MediatR;

    public class ApplyFilterCommand : AddFilterCommand, IRequest<Result>
    {
        public ApplyFilterCommand(IUser viewer) : base(viewer)
        {
        }

        public string InputListMembers { get; set; }

        public string MyWatchlistMembers { get; set; }
    }
}