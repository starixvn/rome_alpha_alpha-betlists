namespace Alpha.BetLists.Site.Requests.CasinoDetails
{
    using BetLists.Models;
    using Fanex.BetList.Core.Entities;
    using MediatR;
    using ViewModels;

    public class CasinoDetailsExcelQuery : CasinoDetailsQuery, IRequest<ExcelFileViewModel>
    {
        public CasinoDetailsExcelQuery(IUser viewer) : base(viewer)
        {
            PageSize = int.MaxValue;
        }

        public RTFHelper? RtfHelper { get; set; }
    }
}
