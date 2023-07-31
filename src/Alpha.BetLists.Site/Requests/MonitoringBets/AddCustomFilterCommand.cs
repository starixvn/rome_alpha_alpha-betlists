namespace Alpha.BetLists.Site.Requests.MonitoringBets
{
    using Alpha.BetLists.Models;
    using Alpha.Web.Models;
    using MediatR;

    public class AddCustomFilterCommand : BaseQuery, IRequest<Result>
    {
        public AddCustomFilterCommand(IUser viewer) : base(viewer)
        {
            IsInternal = viewer.IsInternal;
            SubAccId = viewer.SubAccId;
            LanguageKey = viewer.LangKey;
            IP = viewer.IP;
        }

        public string FilterName { get; set; }

        public bool IsDefault { get; set; }

        public bool IsInternal { get; }

        public int SubAccId { get; }

        public string LanguageKey { get; }

        public string IP { get; }

        public virtual bool IsValid()
            => !string.IsNullOrWhiteSpace(FilterName);
    }
}