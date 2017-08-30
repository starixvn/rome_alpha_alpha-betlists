namespace Alpha.BetLists.Site.Requests.RacingByMatch
{
    using BetLists.Models;
    using Fanex.BetList.Core.Entities;
    using MediatR;
    using ViewModels;

    public class RacingByMatchExcelQuery : RacingByMatchQuery, IRequest<ExcelFileViewModel>
    {
        public RacingByMatchExcelQuery(IUser viewer) : base(viewer)
        {
        }

        public RTFHelper? RtfHelper { get; set; }
    }
}
