namespace Alpha.BetLists.Site.Requests.CasinoDetails
{
    using BetLists.Models;
    using MediatR;
    using ViewModels;

    public class LiveCasinoDetailsExcelQuery : CasinoDetailsExcelQuery, IRequest<ExcelFileViewModel>
    {
        public LiveCasinoDetailsExcelQuery(IUser viewer) : base(viewer)
        {
        }
    }
}
