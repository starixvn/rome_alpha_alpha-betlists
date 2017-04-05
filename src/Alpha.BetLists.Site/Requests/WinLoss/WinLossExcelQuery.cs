namespace Alpha.BetLists.Site.Requests.WinLoss
{
    using BetLists.Models;
    using MediatR;
    using ViewModels;

    public class WinLossExcelQuery : WinLossQuery, IRequest<ExcelFileViewModel>
    {
        public WinLossExcelQuery(IUser viewer) : base(viewer)
        {
        }
    }
}