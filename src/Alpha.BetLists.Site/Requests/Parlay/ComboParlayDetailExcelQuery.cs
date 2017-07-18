namespace Alpha.BetLists.Site.Requests.Parlay
{
    using BetLists.Models;
    using Fanex.BetList.Core.Entities;
    using MediatR;
    using ViewModels;

    public class ComboParlayDetailExcelQuery : ComboParlayDetailQuery, IRequest<ExcelFileViewModel>
    {
        public ComboParlayDetailExcelQuery(IUser viewer) : base(viewer)
        {
        }

        /// <summary>
        /// Use for unit testing
        /// </summary>
        public RTFHelper? RtfHelper { get; set; }
    }
}