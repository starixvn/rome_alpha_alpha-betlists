namespace Alpha.BetLists.Site.Requests.MonitoringBets
{
    using Alpha.BetLists.Models;
    using Alpha.Web.Models;
    using MediatR;

    public class AddFilterCommand : UpdateFilterCommand, IRequest<Result>
    {
        public AddFilterCommand(IUser viewer) : base(viewer)
        {
        }

        public string Bettype { get; set; }

        public int Stake { get; set; }

        public string SporttypeList { get; set; }

        public bool IsLive { get; set; }

        public string InputList { get; set; }

        public string WatchList { get; set; }

        public byte IsWatchList { get; set; }
    }
}