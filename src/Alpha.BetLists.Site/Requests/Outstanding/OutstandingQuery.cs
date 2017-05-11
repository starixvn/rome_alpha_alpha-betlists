namespace Alpha.BetLists.Site.Requests.Outstanding
{
    using BetLists.Models;
    using MediatR;
    using ViewModels.Outstanding;

    public class OutstandingQuery : BaseQuery, IRequest<OutstandingViewModel>
    {
        public OutstandingQuery(IUser viewer) : base(viewer)
        {
        }

        public bool GetAll { get; set; }

        public string UserName { get; set; }
    }
}