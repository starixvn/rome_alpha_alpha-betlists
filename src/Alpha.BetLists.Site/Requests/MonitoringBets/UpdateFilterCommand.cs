namespace Alpha.BetLists.Site.Requests.MonitoringBets
{
    using Alpha.BetLists.Models;
    using Alpha.Web.Models;
    using MediatR;

    public class UpdateFilterCommand : AddCustomFilterCommand, IRequest<Result>
    {
        public UpdateFilterCommand(IUser viewer) : base(viewer)
        {
        }

        public int FilterId { get; set; }

        public override bool IsValid()
            => base.IsValid() && FilterId > 0;
    }
}