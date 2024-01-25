namespace Alpha.BetLists.Site.Requests.LastBets
{
    using BetLists.Models;
    using MediatR;
    using ViewModels.LastBets;

    public class LastBetsQuery : BaseQuery, IRequest<LastBetsViewModel>
    {
        public LastBetsQuery(IUser viewer) : base(viewer)
        {
        }

        public int BetType { get; set; } = -1;

        public int PageSize { get; set; } = 10;

        public bool HidePopup { get; set; }
    }
}