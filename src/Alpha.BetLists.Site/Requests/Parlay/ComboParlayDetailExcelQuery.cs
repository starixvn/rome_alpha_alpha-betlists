namespace Alpha.BetLists.Site.Requests.Parlay
{
    using BetLists.Models;
    using MediatR;
    using ViewModels;

    public class ComboParlayDetailExcelQuery : ComboParlayDetailQuery, IRequest<ExcelFileViewModel>
    {
        public ComboParlayDetailExcelQuery(IUser viewer) : base(viewer)
        {
        }
    }
}