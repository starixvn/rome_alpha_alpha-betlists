using Alpha.BetLists.Models;
using Alpha.BetLists.Site.ViewModels;
using Fanex.BetList.Core.Entities;
using MediatR;

namespace Alpha.BetLists.Site.Requests.CancelledBet
{
    public class CancelledBetExcelQuery : CancelledBetBaseQuery, IRequest<ExcelFileViewModel>
    {
        public CancelledBetExcelQuery(IUser viewer) : base(viewer)
        {
        }

        public RTFHelper? RtfHelper { get; set; }
    }
}