using Alpha.BetLists.Models;
using Alpha.BetLists.Site.ViewModels;
using MediatR;

namespace Alpha.BetLists.Site.Requests.WinLossByMatch
{
    public class ParlayWinLossByMatchExcelQuery : ParlayWinLossByMatchQuery, IRequest<ExcelFileViewModel>
    {
        public ParlayWinLossByMatchExcelQuery(IUser viewer) : base(viewer)
        {
        }
    }
}