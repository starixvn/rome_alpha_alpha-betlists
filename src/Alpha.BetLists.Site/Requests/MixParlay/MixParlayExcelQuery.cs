namespace Alpha.BetLists.Site.Requests.MixParlay
{
    using BetLists.Models;
    using Fanex.BetList.Core.Entities;
    using MediatR;
    using ViewModels;

    public class MixParlayExcelQuery : MixParlayQuery, IRequest<ExcelFileViewModel>
    {
        public MixParlayExcelQuery(IUser viewer) : base(viewer)
        {
        }

        public RTFHelper? RtfHelper { get; set; }
    }
}
