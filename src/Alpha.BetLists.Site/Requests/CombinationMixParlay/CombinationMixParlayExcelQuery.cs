namespace Alpha.BetLists.Site.Requests.CombinationMixParlay
{
    using BetLists.Models;
    using Fanex.BetList.Core.Entities;
    using MediatR;
    using ViewModels;

    public class CombinationMixParlayExcelQuery : CombinationMixParlayQuery, IRequest<ExcelFileViewModel>
    {
        public CombinationMixParlayExcelQuery(IUser viewer) : base(viewer)
        {
        }

        public RTFHelper? RtfHelper { get; set; }
    }
}
