namespace Alpha.BetLists.Services
{
    using System.Threading.Tasks;
    using Alpha.BetLists.Criteria;
    using Fanex.Data.Repository;
    using Models;
    using Params;

    public interface CashOutHistoryService
    {
        Task<CashOutHistoryModel> GetHistory(CashOutHistoryParam param);
    }

    public class CashOutHistoryServiceImpl : CashOutHistoryService
    {
        private readonly ReportDateService reportDateService;
        private readonly IDynamicRepository dynamicRepository;

        public CashOutHistoryServiceImpl(
            ReportDateService reportDateService,
            IDynamicRepository dynamicRepository)
        {
            this.reportDateService = reportDateService;
            this.dynamicRepository = dynamicRepository;
        }

        public async Task<CashOutHistoryModel> GetHistory(CashOutHistoryParam param)
        {
            var balanceUpDay = reportDateService.GetBalanceUpDay();
            var criteria = new GetCashOutHistoryCriteria(param.TransId, param.CustId, param.WinlostDate, balanceUpDay);
            var history = await dynamicRepository.GetAsync<CashOutHistoryModel>(criteria).ConfigureAwait(false);

            return new CashOutHistoryModel
            {
                TransId = param.TransId,
                CashoutExtraInfo = history != null ? history.CashoutExtraInfo : string.Empty
            };
        }
    }
}