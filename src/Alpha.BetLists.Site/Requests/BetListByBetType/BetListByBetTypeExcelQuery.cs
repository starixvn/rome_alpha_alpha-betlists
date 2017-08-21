namespace Alpha.BetLists.Site.Requests.BetListByBetType
{
    using BetLists.Models;
    using Fanex.BetList.Core.Entities;
    using MediatR;
    using ViewModels;

    public class BetListByBetTypeExcelQuery : BetListByBetTypeQuery, IRequest<ExcelFileViewModel>
    {
        public BetListByBetTypeExcelQuery(IUser viewer) : base(viewer)
        {
        }

        public RTFHelper? RtfHelper { get; set; }
    }
}
