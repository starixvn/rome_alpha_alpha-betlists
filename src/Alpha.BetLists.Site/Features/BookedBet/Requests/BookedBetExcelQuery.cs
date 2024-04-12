namespace Alpha.BetLists.Site.Features.BookedBet
{
    using Alpha.BetLists.Models;
    using Alpha.BetLists.Site.ViewModels;
    using Fanex.BetList.Core.Entities;
    using MediatR;

    public class BookedBetExcelQuery : BookedBetBaseQuery, IRequest<ExcelFileViewModel>
    {
        public BookedBetExcelQuery(IUser viewer) : base(viewer)
        {
        }

        public RTFHelper? RtfHelper { get; set; }
    }
}