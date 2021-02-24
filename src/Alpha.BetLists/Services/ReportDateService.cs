namespace Alpha.BetLists.Services
{
    using Criteria;
    using Fanex.Caching;
    using Fanex.Data.Repository;
    using Models;
    using System;

    public interface ReportDateService
    {
        BalanceUpDayAndMoveBetsDay GetBalanceUpDayAndMoveBetsDay();

        DateTime GetBalanceUpDay();

        DateTime GetMoveBetsDay();
    }

    public class ReportDateServiceImpl : ReportDateService
    {
        private const string BalanceUpDayAndMoveBetsDayCacheKey = "Alpha.Reports.BalanceUpDayAndMoveBetsDay";

        private readonly IDynamicRepository dynamicRepository;
        private readonly ICacheService cacheService;

        public ReportDateServiceImpl(IDynamicRepository dynamicRepository, ICacheService cacheService)
        {
            this.dynamicRepository = dynamicRepository;
            this.cacheService = cacheService;
        }

        public static ReportDateService Instance { get; set; }

        public DateTime GetBalanceUpDay() => GetBalanceUpDayAndMoveBetsDay().BalanceUpDay;

        public DateTime GetMoveBetsDay() => GetBalanceUpDayAndMoveBetsDay().MoveBetsDay;

        public BalanceUpDayAndMoveBetsDay GetBalanceUpDayAndMoveBetsDay()
        {
            var balanceUpDayAndMoveBetsDay = cacheService.GetOrSet(
                BalanceUpDayAndMoveBetsDayCacheKey,
                () =>
                {
                    var reportDates = dynamicRepository.Get<BalanceUpDayAndMoveBetsDay>(
                        new GetBalanceUpDayAndMoveBetsDayCriteria());

                    return reportDates;
                },
                new CacheItemOptions().SetAbsoluteExpiration(TimeSpan.FromMinutes(5)));

            return balanceUpDayAndMoveBetsDay;
        }
    }
}
