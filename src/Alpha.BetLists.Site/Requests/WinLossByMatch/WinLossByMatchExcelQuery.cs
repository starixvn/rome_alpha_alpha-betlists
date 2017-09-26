namespace Alpha.BetLists.Site.Requests.WinLossByMatch
{
    using BetLists.Models;
    using Fanex.BetList.Core.Entities;
    using MediatR;
    using ViewModels;

    public class WinLossByMatchExcelQuery : WinLossByMatchQuery, IRequest<ExcelFileViewModel>
    {
        public WinLossByMatchExcelQuery(IUser viewer) : base(viewer)
        {
        }

        /// <summary>
        /// Use for unit testing
        /// </summary>
        public RTFHelper? RtfHelper { get; set; }
    }
}
