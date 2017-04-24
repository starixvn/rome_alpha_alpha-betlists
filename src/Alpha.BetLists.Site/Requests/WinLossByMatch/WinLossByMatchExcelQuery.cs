using Alpha.BetLists.Models;
using Alpha.BetLists.Site.ViewModels;
using MediatR;

namespace Alpha.BetLists.Site.Requests.WinLossByMatch
{
    public class WinLossByMatchExcelQuery : WinLossByMatchQuery, IRequest<ExcelFileViewModel>
    {
        public WinLossByMatchExcelQuery(IUser viewer) : base(viewer)
        {
        }
    }
}