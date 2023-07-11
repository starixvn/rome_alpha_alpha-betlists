namespace Alpha.BetLists.Site.Requests.MonitoringBets
{
    using Alpha.BetLists.Constants;
    using Alpha.BetLists.Models;
    using Alpha.BetLists.Models.MonitoringBets;
    using Alpha.Web.Models;
    using MediatR;

    public class AddFilterCommand : UpdateFilterCommand, IRequest<Result>
    {
        public AddFilterCommand(IUser viewer) : base(viewer)
        {
        }

        public string BetType { get; set; }

        public int Stake { get; set; }

        public SportXml[] Sports { get; set; }

        public byte IsLive { get; set; }

        public byte FilterByMembers { get; set; }

        public string InputList { get; set; }

        public string MyWatchlist { get; set; }

        public override bool IsValid()
        {
            return !string.IsNullOrWhiteSpace(FilterName)
                && !string.IsNullOrWhiteSpace(BetType)
                && Stake > 0
                && Sports?.Length > 0
                && (FilterByMembers == FilterByMembersConstants.AllMembers
                || FilterByMembers == FilterByMembersConstants.AllWatchlistMembers
                || FilterByMembers == FilterByMembersConstants.SpecificMembers)
                && (!string.IsNullOrWhiteSpace(InputList) || !string.IsNullOrWhiteSpace(MyWatchlist));
        }
    }
}