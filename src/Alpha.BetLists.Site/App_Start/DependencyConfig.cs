namespace Alpha.BetLists.Site.App_Start
{
    using Alpha.BetLists.Site.Handlers.RiskAnalysisIPMatching;
    using Alpha.IPInfo.DataAccess;
    using Alpha.IPInfo.Models;
    using Alpha.IPInfo.Services;
    using Alpha.NetThrottling;
    using Alpha.NetThrottling.Models;
    using Alpha.NetThrottling.Resolvers;
    using Alpha.NetThrottling.Utils;
    using Alpha.PartialUM.Helpers;
    using Alpha.Web.Helpers;
    using AlphaPlus.PartialUM.DataAccess;
    using AlphaPlus.PartialUM.Helpers;
    using AlphaPlus.PartialUM.Services;
    using AlphaPlus.SmartPunter.DataAccesses;
    using AlphaPlus.SmartPunter.Services;
    using AlphaPlus.User.Models;
    using AlphaPlus.Utilities.IP;
    using BetLists.Helpers;
    using BetLists.Models;
    using Controllers;
    using DataAccesses;
    using Fanex.BetDetails.BetBuilder.DataAccesses;
    using Fanex.BetDetails.BetBuilder.Services;
    using Fanex.BetDetails.Casino.DataAccesses;
    using Fanex.BetDetails.Casino.Services;
    using Fanex.BetList.Core.Builder;
    using Fanex.BetList.Core.Entities;
    using Fanex.BetList.Core.Factories;
    using Fanex.BetList.Core.RemotingRef;
    using Fanex.BetList.Core.TicketHelper;
    using Fanex.Caching;
    using Fanex.Caching.Redis;
    using Fanex.Data.Repository;
    using Fanex.RemotingRef;
    using Handlers.Parlay;
    using MediatR;
    using Models;
    using ONEbook.SpreadSheets;
    using ONEbook.SpreadSheets.BetListNPOI;
    using Services;
    using SimpleInjector;
    using SimpleInjector.Integration.Web;
    using SimpleInjector.Integration.Web.Mvc;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Reflection;
    using System.Web.Mvc;
    using AppSettings = Configurations.AppSettings;

    public static class DependencyConfig
    {
        [SuppressMessage("Microsoft.Reliability", "CA2000:Dispose objects before losing scope", Justification = "Reviewed")]
        public static void Init()
        {
            UserSession.Init(new HttpContextHelper());

            var container = new Container();

            container.Options.DefaultScopedLifestyle = new WebRequestLifestyle();

            RegisterMediator(container);

            RegisterDataAccesses(container);

            RegisterServices(container);

            RegisterBetListCore(container);

            RegisterNetThrottling(container);

            // This is an extension method from the integration package.
            container.RegisterMvcControllers(Assembly.GetExecutingAssembly());

            // This is an extension method from the integration package as well.
            container.RegisterMvcIntegratedFilterProvider();

            container.Verify();

            DependencyResolver.SetResolver(new SimpleInjectorDependencyResolver(container));

            RegisterStaticInstances(container);
        }

        private static void RegisterMediator(Container container)
        {
            var assemblies = GetAssemblies();
            container.RegisterSingleton<IMediator, Mediator>();
            container.Register(typeof(IRequestHandler<,>), assemblies);
            container.Register(typeof(IAsyncRequestHandler<,>), assemblies);
            container.Register(typeof(ICancellableAsyncRequestHandler<>), assemblies);
            container.RegisterCollection(typeof(INotificationHandler<>), assemblies);
            container.RegisterCollection(typeof(IAsyncNotificationHandler<>), assemblies);
            container.RegisterCollection(typeof(ICancellableAsyncNotificationHandler<>), assemblies);
            container.RegisterCollection(typeof(IPipelineBehavior<,>), assemblies);

            container.RegisterSingleton(new SingleInstanceFactory(container.GetInstance));
            container.RegisterSingleton(new MultiInstanceFactory(container.GetAllInstances));
        }

        private static IEnumerable<Assembly> GetAssemblies()
        {
            yield return typeof(WinLossController).Assembly;
        }

        private static void RegisterBetListCore(Container container)
        {
            container.RegisterPerWebRequest<ITicketHelper, TicketHelper>();
            container.Register(typeof(Workbook<>), typeof(NpoiWorkbook<>), Lifestyle.Scoped);
            container.RegisterPerWebRequest<IBetListFactory<BetListHTMLBuilder>, BetListHTMLFactory>();
            container.RegisterPerWebRequest<IBetListFactory<BetListExcelBuilder>, BetListExcelFactory>();
            container.RegisterPerWebRequest<IBetListHTMLBuilder>(() => container.GetInstance<IBetListFactory<BetListHTMLBuilder>>().CreateBuilder());
            container.RegisterPerWebRequest<IBetListExcelBuilder>(() => container.GetInstance<IBetListFactory<BetListExcelBuilder>>().CreateBuilder());
            container.RegisterSingleton<ComboParlayDetailWorkbook, ComboParlayDetailWorkbookImpl>();
        }

        private static void RegisterDataAccesses(Container container)
        {
            container.Register<IDynamicRepository, DynamicRepository>(Lifestyle.Singleton);
            container.Register<ISmartPunterDataAccess, SmartPunterDataAccess>(Lifestyle.Singleton);
            container.Register<IBetBuilderDataAccess, BetBuilderDataAccess>(Lifestyle.Singleton);
            container.Register<ICasinoDetailsDataAccess, CasinoDetailsDataAccess>(Lifestyle.Singleton);
            container.Register<IPartialUMDataAccess, PartialUMDataAccess>(Lifestyle.Singleton);
            container.Register<IIPInfoDataAccess, IPInfoDataAccess>(Lifestyle.Singleton);
            container.Register<IBetListRepository, BetListRepository>(Lifestyle.Singleton);
        }

        private static void RegisterServices(Container container)
        {
            container.Register<Helpers.BrowserHelper, Helpers.BrowserHelperImpl>(Lifestyle.Singleton);
            container.RegisterSingleton<IUserSession, UserSessionExt>();
            container.Register<WinLossService, WinLossServiceImpl>(Lifestyle.Singleton);
            container.Register<ReportDateService, ReportDateServiceImpl>(Lifestyle.Singleton);
            container.Register<ParlayService, ParlayServiceImpl>(Lifestyle.Singleton);
            container.Register<ISmartPunterService, SmartPunterService>(Lifestyle.Singleton);
            container.Register(
                () => SmartPunterBetListMonitor.CreateInstance(
                    container.GetInstance<IUserSession>(),
                    container.GetInstance<ISmartPunterService>(),
                    Helpers.IPHelper.CustomerIP),
                Lifestyle.Singleton);

            container.Register<IPService, IPServiceImpl>(Lifestyle.Singleton);

            container.Register<WinLossByMatchService, WinLossByMatchServiceImpl>(Lifestyle.Singleton);
            container.Register<ParlayWinLossByMatchService, ParlayWinLossByMatchServiceImpl>(Lifestyle.Singleton);
            container.Register<RunningBetListService, RunningBetListServiceImpl>(Lifestyle.Singleton);
            container.Register<IBetBuilderService, BetBuilderService>(Lifestyle.Singleton);
            container.Register<SearchTicketsService, SearchTicketsServiceImpl>(Lifestyle.Singleton);

            container.Register<LastBetsService, LastBetsServiceImpl>(Lifestyle.Singleton);
            container.Register<EncryptionService, EncryptionServiceImpl>(Lifestyle.Singleton);

            container.Register<MemberBetListService, MemberBetListServiceImpl>(Lifestyle.Singleton);
            container.Register<DailyStatementService, DailyStatementServiceImpl>(Lifestyle.Singleton);

            container.RegisterSingleton<ICacheRef, CacheRef>();
            container.RegisterSingleton<IRemotingRefService, RemotingRefService>();
            container.RegisterSingleton<ICacheService>(new CacheService());
            container.Register<ForecastService, ForecastServiceImpl>(Lifestyle.Singleton);
            container.Register<ProductService>(
                () => new ProductServiceImpl(
                    container.GetInstance<IDynamicRepository>(),
                    container.GetInstance<ICacheService>(),
                    UserPermission.IsShowOrNotExistProduct,
                    CacheRefServiceHelper.GetProductName),
                Lifestyle.Singleton);
            container.RegisterSingleton<ProductDetailService>(
                () => new ProductDetailServiceImpl(
                    CacheRefServiceHelper.GetProductDetailName,
                    container.GetInstance<ProductService>().CanViewProduct,
                    container.GetInstance<IDynamicRepository>(),
                    container.GetInstance<ICacheService>()));
            container.Register<CancelledBetService, CancelledBetServiceImpl>();

            container.Register<ICasinoDetailsService, CasinoDetailsService>(Lifestyle.Singleton);
            container.Register<CorrectScoreService, CorrectScoreServiceImpl>(Lifestyle.Singleton);
            container.Register<RunningBetListByBetTypeService, RunningBetListByBetTypeServiceImpl>(Lifestyle.Singleton);
            container.Register<RunningBetListByBetTypeAndMatchService, RunningBetListByBetTypeAndMatchServiceImpl>(Lifestyle.Singleton);
            container.Register<JackpotDetailsService, JackpotDetailsServiceImpl>(Lifestyle.Singleton);
            container.Register<RiskAnalysisIPMatchingService, RiskAnalysisIPMatchingServiceImpl>(Lifestyle.Singleton);
            container.Register<RiskAnalysisIPMatchingHelper, RiskAnalysisIPMatchingHelperImpl>(Lifestyle.Singleton);
            container.Register<CashOutHistoryService, CashOutHistoryServiceImpl>(Lifestyle.Singleton);

            container.Register<MoveStatementHistoryService, MoveStatementHistoryServiceImpl>(Lifestyle.Singleton);

            container.RegisterSingleton<IUser, User>();

            container.RegisterSingleton<IPartialUMSectionHelper, PartialUMSectionHelper>();
            container.RegisterSingleton<IPartialUMService, PartialUMService>();
            container.RegisterSingleton<AutoFollowBetService, AutoFollowBetServiceImpl>();
            container.Register<IIPInfoService>(
                () => new IPInfoService(
                    new IPInformationConfig
                    {
                        IPV4TopCountries = AppSettings.IPV4TopCountries,
                        IPV4TopCountriesCacheInMinutes = AppSettings.IPV4TopCountriesCacheInMinutes,
                        IPV4CacheInMinutes = AppSettings.IPV4CacheInMinutes,
                        IPV6CacheInMinutes = AppSettings.IPV6CacheInMinutes
                    },
                    GetCacheServiceInstance(AppSettings.IPInfoCommonCacheRedis),
                    GetCacheServiceInstance(AppSettings.IPInfoIndividualCacheRedis),
                    container.GetInstance<IIPInfoDataAccess>()),
                Lifestyle.Singleton);
            container.RegisterSingleton<MonitoringBetsService, MonitoringBetsServiceImpl>();
            container.RegisterSingleton<UsernameSuggestionService, UsernameSuggestionServiceImpl>();
        }

        private static void RegisterStaticInstances(Container container)
        {
            ReportDateServiceImpl.Instance = container.GetInstance<ReportDateService>();
        }

        private static void RegisterNetThrottling(Container container)
        {
            container.Register<INetThrottler>(() => new NetThrottler(false));
            container.Register<IThrottleSetting>(() => new ThrottleSetting(false));
            container.Register<IClock, Clock>(Lifestyle.Scoped);
            container.Register<ILogger, Logger>(Lifestyle.Scoped);
            container.Register<ICacheServiceFactory, CacheServiceFactory>(Lifestyle.Scoped);
            container.Register<IResolver, BaseResolver>(Lifestyle.Scoped);
        }

        private static ICacheService GetCacheServiceInstance(string redisConnectionString)
            => string.IsNullOrWhiteSpace(redisConnectionString)
            ? new CacheService()
            : new CacheService(new RedisCache(new RedisCacheOptions
            {
                ConnectionString = redisConnectionString,
                ApplicationName = "Alpha-BetLists"
            }));
    }
}