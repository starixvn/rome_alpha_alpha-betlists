namespace Alpha.BetLists.Site.Requests.CasinoDetails
{
    using BetLists.Models;
    using MediatR;
    using ViewModels;

    public class RNGCasinoDetailsExcelQuery : CasinoDetailsExcelQuery, IRequest<ExcelFileViewModel>
    {
        public RNGCasinoDetailsExcelQuery(IUser viewer) : base(viewer)
        {
        }
    }
}
