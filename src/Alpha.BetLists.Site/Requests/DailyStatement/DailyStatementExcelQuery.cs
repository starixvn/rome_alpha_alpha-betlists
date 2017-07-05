namespace Alpha.BetLists.Site.Requests.DailyStatement
{
    using BetLists.Models;
    using Fanex.BetList.Core.Entities;
    using MediatR;
    using ViewModels;

    public class DailyStatementExcelQuery : DailyStatementQuery, IRequest<ExcelFileViewModel>
    {
        public DailyStatementExcelQuery(IUser viewer) : base(viewer)
        {
        }

        /// <summary>
        /// Use for unit testing
        /// </summary>
        public RTFHelper? RtfHelper { get; set; }
    }
}
