namespace Alpha.BetLists.Site.Requests.MonitoringBets
{
    using Alpha.BetLists.Site.ViewModels.MonitoringBets;
    using BetLists.Models;
    using MediatR;

    public class GetLastFilteringBetsQuery : BaseQuery, IRequest<FilteringBetDataViewModel>
    {
        public GetLastFilteringBetsQuery(IUser viewer) : base(viewer)
        {
            SubAccId = viewer.SubAccId;
        }

        public int SubAccId { get; }

        public int FilterId { get; set; }

        public long TransId { get; set; }

        public string MemberList { get; set; }

        public string BetTypeList { get; set; }

        public string SportTypeList { get; set; }

        public string LeagueList { get; set; }

        public byte EvenStatus { get; set; }

        public decimal MinStake { get; set; }

        public int LimitRowCount { get; set; }

        public long LatestTransId { get; set; }

        public byte FilterByMembers { get; set; }
    }
}