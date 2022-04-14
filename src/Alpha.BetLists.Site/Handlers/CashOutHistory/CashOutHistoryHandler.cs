namespace Alpha.BetLists.Site.Handlers.JackpotDetails
{
    using System.Threading.Tasks;
    using Alpha.BetLists.Site.Requests.CasinoDetails;
    using Alpha.BetLists.Site.ViewModels.CashOutHistory;
    using MediatR;
    using Params;
    using Services;

    public class CashOutHistoryHandler : IAsyncRequestHandler<CashOutHistoryQuery, CashOutHistoryViewModel>
    {
        private readonly CashOutHistoryService cashOutHistoryService;

        public CashOutHistoryHandler(
            CashOutHistoryService cashoutHistoryService)
        {
            this.cashOutHistoryService = cashoutHistoryService;
        }

        public async Task<CashOutHistoryViewModel> Handle(CashOutHistoryQuery message)
        {
            var param = new CashOutHistoryParam(message.TransId, message.CustId, message.WinLostDate);
            var cashOutHistory = await cashOutHistoryService.GetHistory(param);

            return new CashOutHistoryViewModel(cashOutHistory);
        }
    }
}