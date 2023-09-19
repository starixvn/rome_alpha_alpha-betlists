namespace Alpha.BetLists.Site.Requests.MonitoringBets
{
    using Alpha.BetLists.Models;
    using Alpha.Web.Models;
    using MediatR;

    public class AddUnsavedFilterCommand : BaseQuery, IRequest<Result>
    {
        public AddUnsavedFilterCommand(IUser viewer) : base(viewer)
        {
            IsInternal = viewer.IsInternal;
            LanguageKey = viewer.LangKey;
            IP = viewer.IP;
        }

        public string FilterName { get; set; }

        public bool IsDefault { get; set; }

        public bool IsInternal { get; }

        public string LanguageKey { get; }

        public string IP { get; }

        public virtual bool IsValid()
            => !string.IsNullOrWhiteSpace(FilterName);
    }
}