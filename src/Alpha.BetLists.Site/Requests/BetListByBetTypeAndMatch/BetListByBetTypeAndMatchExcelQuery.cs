namespace Alpha.BetLists.Site.Requests.BetListByBetTypeAndMatch
{
    using BetLists.Models;
    using Fanex.BetList.Core.Entities;
    using MediatR;
    using ViewModels;

    public class BetListByBetTypeAndMatchExcelQuery : BetListByBetTypeAndMatchQuery, IRequest<ExcelFileViewModel>
    {
        public BetListByBetTypeAndMatchExcelQuery(IUser viewer) : base(viewer)
        {
        }

        public RTFHelper? RtfHelper { get; set; }
    }
}
