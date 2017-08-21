namespace Alpha.BetLists.Site.Requests.CorrectScore
{
    using BetLists.Models;
    using Fanex.BetList.Core.Entities;
    using MediatR;
    using ViewModels;

    public class CorrectScoreExcelQuery : CorrectScoreQuery, IRequest<ExcelFileViewModel>
    {
        public CorrectScoreExcelQuery(IUser viewer) : base(viewer)
        {
        }

        public RTFHelper? RtfHelper { get; set; }
    }
}
