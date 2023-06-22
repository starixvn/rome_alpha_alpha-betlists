namespace Alpha.BetLists.Site.Requests.MonitoringBets
{
    using Alpha.BetLists.Models;
    using Alpha.Web.Models;
    using MediatR;

    public class UpdateFilterCommand : BaseQuery, IRequest<Result>
    {
        public UpdateFilterCommand(IUser viewer) : base(viewer)
        {
            IsInternal = viewer.IsInternal;
            SubAccId = viewer.SubAccId;
            LanguageKey = viewer.LangKey;
            IP = viewer.IP;
        }

        public int FilterId { get; set; }

        public string FilterName { get; set; }

        public bool IsDefault { get; set; }

        public bool IsInternal { get; }

        public int SubAccId { get; }

        public string LanguageKey { get; }

        public string IP { get; }
    }
}