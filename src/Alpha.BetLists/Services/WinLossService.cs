namespace Alpha.BetLists.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Alpha.BetLists.AppResources;
    using Alpha.PartialUM.Services;
    using Criteria.WinLossByDate;
    using Extensions;
    using Fanex.BetList.Core.Entities;
    using Fanex.Data.Repository;
    using Models;
    using Params;

    public interface WinLossService
    {
        Task<BetListData> GetBetList(WinLossParam param);
    }

    public class WinLossServiceImpl : WinLossService
    {
        private readonly ReportDateService reportDateService;
        private readonly IDynamicRepository dynamicRepository;
        private readonly IPartialUMService partialUMService;

        public WinLossServiceImpl(
            ReportDateService reportDateService,
            IDynamicRepository dynamicRepository,
            IPartialUMService partialUMService)
        {
            this.reportDateService = reportDateService;
            this.dynamicRepository = dynamicRepository;
            this.partialUMService = partialUMService;
        }

        public async Task<BetListData> GetBetList(WinLossParam param)
        {
            var balanceUpDate = reportDateService.GetBalanceUpDay();
            var previousBalanceUpDate = balanceUpDate.AddDays(-1);
            Tuple<IEnumerable<TicketDto>, IEnumerable<TicketData>, BetlistOutputParams> betListDataTuple;
            var fromDate = param.FromDate.GetGreaterDate(param.LimitedDate);

            if (fromDate.IsBefore(balanceUpDate))
            {
                if (param.ToDate.IsBeforeOrEqual(previousBalanceUpDate))
                {
                    betListDataTuple = await GetBetListBeforeBalanceUpDay(param, fromDate: fromDate, toDate: param.ToDate);
                }
                else
                {
                    betListDataTuple = await GetBetList(param, fromDate: balanceUpDate);
                    var bettrans14Tickets = betListDataTuple.Item3.ErrCode != 0 ?
                        betListDataTuple.Item3.MaxTotalRow : betListDataTuple.Item1.Count();

                    Tuple<IEnumerable<TicketDto>, IEnumerable<TicketData>, BetlistOutputParams> oldBetListDataTuple;

                    if (bettrans14Tickets <= param.MaxTotalRowConfig)
                    {
                        param.MaxTotalRow = (param.MaxTotalRow - bettrans14Tickets) + 1;
                    }
                        
                    oldBetListDataTuple = await GetBetListBeforeBalanceUpDay(param, fromDate: fromDate, toDate: previousBalanceUpDate);
                    var totalTickets = (oldBetListDataTuple.Item3.ErrCode != 0 ? 
                        oldBetListDataTuple.Item3.MaxTotalRow : oldBetListDataTuple.Item1.Count()) + bettrans14Tickets;

                    if (totalTickets > param.MaxTotalRowConfig)
                    {
                        if (!param.IsExcel)
                        {
                            betListDataTuple = oldBetListDataTuple = new Tuple<IEnumerable<TicketDto>, IEnumerable<TicketData>, BetlistOutputParams>(
                                Enumerable.Empty<TicketDto>(),
                                Enumerable.Empty<TicketData>(),
                                new BetlistOutputParams(100,
                                    string.Format(Resources.ExceededMaxTotalRowWithExcelFunc, totalTickets, param.MaxTotalRowConfig),
                                    param.MaxTotalRowConfig));
                        }
                        else
                        {
                            var remainingTickets = param.MaxTotalRowConfig - bettrans14Tickets;
                            var ticketDto = oldBetListDataTuple.Item1.Take(remainingTickets);
                            var ticketData = oldBetListDataTuple.Item2.Take(remainingTickets);
                            oldBetListDataTuple = new Tuple<IEnumerable<TicketDto>, IEnumerable<TicketData>, BetlistOutputParams>(
                                ticketDto,
                                ticketData,
                                new BetlistOutputParams(100,
                                    string.Format(Resources.ExceededMaxTotalRowExportExcelFunc, param.MaxTotalRowConfig),
                                    param.MaxTotalRowConfig));
                        }
                    }

                    return new BetListData(
                        betListDataTuple.Item1.Concat(oldBetListDataTuple.Item1).ToList(),
                        betListDataTuple.Item2.Concat(oldBetListDataTuple.Item2).ToList<ITicketData>(),
                        oldBetListDataTuple.Item3);
                }
            }
            else
            {
                betListDataTuple = await GetBetList(param, fromDate);
            }

            return new BetListData(betListDataTuple.Item1.ToList(), betListDataTuple.Item2.ToList<ITicketData>(), betListDataTuple.Item3);
        }

        private async Task<Tuple<IEnumerable<TicketDto>, IEnumerable<TicketData>, BetlistOutputParams>> GetBetList(WinLossParam param, DateTime fromDate)
        {
            var criteria = new GetBetListWinLossByDateCriteria(
                            param.CustId,
                            param.ProductIds.JoinWithComma(),
                            param.ProductDetailsXML,
                            fromDate,
                            param.ToDate,
                            reportDateService.GetMoveBetsDay(),
                            maxTotalRow: param.MaxTotalRow,
                            isExportExcel: param.IsExcel);

            partialUMService.ThrowIfUM(criteria.GetSettingKey());

            var result = await dynamicRepository.FetchMultipleAsync<TicketDto, TicketData>(criteria);
            var errFormat = param.IsExcel ? 
                string.Format(Resources.ExceededMaxTotalRowExportExcelFunc, param.MaxTotalRowConfig) : 
                string.Format(Resources.ExceededMaxTotalRowWithExcelFunc, criteria.MaxTotalRow, param.MaxTotalRowConfig);

            return new Tuple<IEnumerable<TicketDto>, IEnumerable<TicketData>, BetlistOutputParams>(
                result.Item1,
                result.Item2,
                new BetlistOutputParams(criteria.ErrCode, criteria.ErrCode != 0 ? 
                    errFormat : criteria.ErrMsg, criteria.MaxTotalRow));
        }

        private async Task<Tuple<IEnumerable<TicketDto>, IEnumerable<TicketData>, BetlistOutputParams>> GetBetListBeforeBalanceUpDay(WinLossParam param, DateTime fromDate, DateTime toDate)
        {
            var criteria = new GetBetListWinLossByDateOldCriteria(
                            param.CustId,
                            param.ProductIds.JoinWithComma(),
                            param.ProductDetailsXML,
                            fromDate,
                            toDate,
                            param.Language,
                            maxTotalRow: param.MaxTotalRow,
                            isExportExcel: param.IsExcel);

            partialUMService.ThrowIfUM(criteria.GetSettingKey());

            var result = await dynamicRepository.FetchMultipleAsync<TicketDto, TicketData>(criteria);
            var errFormat = param.IsExcel ?
                string.Format(Resources.ExceededMaxTotalRowExportExcelFunc, param.MaxTotalRowConfig) :
                string.Format(Resources.ExceededMaxTotalRowWithExcelFunc, criteria.MaxTotalRow, param.MaxTotalRowConfig);

            return new Tuple<IEnumerable<TicketDto>, IEnumerable<TicketData>, BetlistOutputParams>(
                result.Item1,
                result.Item2,
                new BetlistOutputParams(criteria.ErrCode, criteria.ErrCode != 0 ?
                    errFormat : criteria.ErrMsg, criteria.MaxTotalRow));
        }
    }
}