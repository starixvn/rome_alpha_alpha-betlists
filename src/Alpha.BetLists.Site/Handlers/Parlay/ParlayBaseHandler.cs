namespace Alpha.BetLists.Site.Handlers
{
    using System.Threading.Tasks;
    using Fanex.BetList.Core.Builder;
    using Fanex.BetList.Core.Entities;
    using Requests.Parlay;
    using Services;
    using SmartPunter.Services;
    using Utilities.IP;
    using ViewModels.Parlay;

    public class ParlayBaseHandler : BetListBaseHandler
    {
        public ParlayBaseHandler(
            ParlayService parlayService,
            IBetListHTMLBuilder builder,
            IBetListExcelBuilder excelBuilder,
            ITicketHelper ticketHelper,
            ISmartPunterBetListMonitor monitorService,
            IPService ipService)
            : base(builder, excelBuilder, ticketHelper, monitorService, ipService)
        {
            ParlayService = parlayService;
        }

        protected ParlayService ParlayService { get; }

        protected async Task<ComboParlayDetailViewModel> GetComboParlayDetail(ComboParlayDetailQuery message)
        {
            var comboParlayDetailData = await ParlayService.GetComboParlayDetail(
                message.TransId,
                message.RefNo,
                message.BetId,
                message.CustId,
                message.Language);

            return new ComboParlayDetailViewModel(message, comboParlayDetailData, TicketHelper);
        }
    }
}