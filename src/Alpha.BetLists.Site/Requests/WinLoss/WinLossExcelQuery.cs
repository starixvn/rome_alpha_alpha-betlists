namespace Alpha.BetLists.Site.Requests.WinLoss
{
    using BetLists.Models;
    using Fanex.BetList.Core.Entities;
    using MediatR;
    using ViewModels;

    public class WinLossExcelQuery : WinLossQuery, IRequest<ExcelFileViewModel>
    {
        public WinLossExcelQuery(IUser viewer) : base(viewer)
        {
        }

        /// <summary>
        /// Use for unit testing
        /// </summary>
        public RTFHelper? RtfHelper { get; set; }
    }
}
