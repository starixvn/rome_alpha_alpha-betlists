namespace Alpha.BetLists.Site.Requests.BullfightingByMatch
{
    using BetLists.Models;
    using Fanex.BetList.Core.Entities;
    using MediatR;
    using ViewModels;

    public class BullfightingByMatchExcelQuery : BullfightingByMatchQuery, IRequest<ExcelFileViewModel>
    {
        public BullfightingByMatchExcelQuery(IUser viewer) : base(viewer)
        {
        }

        public RTFHelper? RtfHelper { get; set; }
    }
}
