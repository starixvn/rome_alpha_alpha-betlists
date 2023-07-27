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

        public byte LiveStatus { get; set; }

        public byte FilterByMembers { get; set; }

        public string InputList { get; set; }

        public string MyWatchlist { get; set; }

        public override bool IsValid()
            => !string.IsNullOrWhiteSpace(FilterName)
                && !string.IsNullOrWhiteSpace(BetType)
                && Stake >= 0
                && IsValidLiveStatus()
                && Sports?.Length > 0
                && IsValidFilterByMembers();

        private bool IsValidLiveStatus()
            => LiveStatus == LiveStatusConstants.All
            || LiveStatus == LiveStatusConstants.Live
            || LiveStatus == LiveStatusConstants.NonLive
            || LiveStatus == LiveStatusConstants.Mix;

        private bool IsValidFilterByMembers()
        {
            if (FilterByMembers == FilterByMembersConstants.AllMembers)
            {
                return InputList == "0" && MyWatchlist == "0";
            }

            if (FilterByMembers == FilterByMembersConstants.AllWatchlistMembers)
            {
                return MyWatchlist == "0";
            }

            return !string.IsNullOrWhiteSpace(InputList) || !string.IsNullOrWhiteSpace(MyWatchlist);
        }
    }
}