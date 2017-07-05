using Alpha.BetLists.Models;
using Alpha.BetLists.Site.ViewModels;
using Fanex.BetList.Core.Entities;
using MediatR;

namespace Alpha.BetLists.Site.Requests.WinLossByMatch
{
    public class ParlayWinLossByMatchExcelQuery : ParlayWinLossByMatchQuery, IRequest<ExcelFileViewModel>
    {
        public ParlayWinLossByMatchExcelQuery(IUser viewer) : base(viewer)
        {
        }

        /// <summary>
        /// Use for unit testing
        /// </summary>
        public RTFHelper? RtfHelper { get; set; }
    }
}
