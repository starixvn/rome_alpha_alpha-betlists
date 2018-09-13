namespace Alpha.BetLists.Site.App_Start
{
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Reflection;
    using System.Web.Mvc;
    using BetLists.Helpers;
    using BetLists.Models;
    using Configurations;
    using Controllers;
    using DataAccesses;
    using Fanex.BetDetails.Casino.DataAccesses;
    using Fanex.BetDetails.Casino.Services;
    using Fanex.BetDetails.Lucky3.DataAccesses;
    using Fanex.BetDetails.Lucky3.Services;
    using Fanex.BetList.Core.Builder;
    using Fanex.BetList.Core.Entities;
    using Fanex.BetList.Core.Factories;
    using Fanex.BetList.Core.RemotingRef;
    using Fanex.BetList.Core.TicketHelper;
    using Fanex.Data.Repository;
    using Fanex.RemotingRef;
    using Handlers.Parlay;
    using Helpers;
    using MediatR;
    using Models;
    using ONEbook.SpreadSheets;
    using ONEbook.SpreadSheets.NPOI;
    using Services;
    using SimpleInjector;
    using SimpleInjector.Integration.Web;
    using SimpleInjector.Integration.Web.Mvc;
    using SmartPunter.DataAccesses;
    using SmartPunter.Services;
    using StaticData.Models;
    using Sunplus.Agent.Model.Base;
    using Sunplus.Agent.Model.Entities;
    using Sunplus.Agent.Model.Entities.Permission;
    using Utilities.IP;

    public static class DependencyConfig
    {
        [SuppressMessage("Microsoft.Reliability", "CA2000:Dispose objects before losing scope", Justification = "Reviewed")]
        public static void Init()
        {
            var container = new Container();

            container.Options.DefaultScopedLifestyle = new WebRequestLifestyle();

            RegisterMediator(container);

            RegisterDataAccesses(container);

            RegisterServices(container);

            RegisterBetListCore(container);

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
            if (AppSettings.EnabledStaticData)
            {
                StaticData.AppStart.DependencyRegister.Register(container);
            }
            else
            {
                container.Register<IDynamicRepository, DynamicRepository>(Lifestyle.Singleton);
            }

            container.Register<ISmartPunterDataAccess, SmartPunterDataAccess>(Lifestyle.Singleton);
            container.Register<ILucky3DataAccess, Lucky3DataAccess>(Lifestyle.Singleton);
            container.Register<ICasinoDetailsDataAccess, CasinoDetailsDataAccess>(Lifestyle.Singleton);
            container.Register<ProductDataAccess, ProductDataAccessImpl>(Lifestyle.Singleton);
        }

        private static void RegisterServices(Container container)
        {
            container.RegisterSingleton<IUserSession, UserSessionExt>();
            container.Register<WinLossService, WinLossServiceImpl>(Lifestyle.Singleton);
            container.Register<ReportDateService, ReportDateServiceImpl>(Lifestyle.Singleton);
            container.Register<ParlayService, ParlayServiceImpl>(Lifestyle.Singleton);
            container.Register<ISmartPunterService, SmartPunterService>(Lifestyle.Singleton);
            container.Register(
                () => SmartPunterBetListMonitor.CreateInstance(
                    container.GetInstance<IUserSession>(),
                    container.GetInstance<ISmartPunterService>(),
                    IPHelper.CustomerIP),
                Lifestyle.Singleton);

            container.Register<IPService, Services.IPServiceImpl>(Lifestyle.Singleton);

            container.Register<WinLossByMatchService, WinLossByMatchServiceImpl>(Lifestyle.Singleton);
            container.Register<ParlayWinLossByMatchService, ParlayWinLossByMatchServiceImpl>(Lifestyle.Singleton);
            container.Register<RunningBetListService, RunningBetListServiceImpl>(Lifestyle.Singleton);
            container.Register<ILucky3Service, Lucky3Service>(Lifestyle.Singleton);

            container.Register<LastBetsService, LastBetsServiceImpl>(Lifestyle.Singleton);
            container.Register<EncryptionService, EncryptionServiceImpl>(Lifestyle.Singleton);

            container.Register<MemberBetListService, MemberBetListServiceImpl>(Lifestyle.Singleton);
            container.Register<DailyStatementService, DailyStatementServiceImpl>(Lifestyle.Singleton);

            container.RegisterSingleton<ICacheRef, CacheRef>();
            container.RegisterSingleton<IRemotingRefService, RemotingRefService>();
            container.Register<ForecastService, ForecastServiceImpl>(Lifestyle.Singleton);
            container.Register<ProductService>(
                () => new ProductServiceImpl(
                    container.GetInstance<IDynamicRepository>(),
                    container.GetInstance<ProductDataAccess>(),
                    UserPermission.HasPermissionOnProduct,
                    CacheRefServiceHelper.GetProductName),
                Lifestyle.Singleton);
            container.Register<CancelledBetService, CancelledBetServiceImpl>();

            container.Register<ICasinoDetailsService, CasinoDetailsService>(Lifestyle.Singleton);
            container.Register<CorrectScoreService, CorrectScoreServiceImpl>(Lifestyle.Singleton);
            container.Register<RunningBetListByBetTypeService, RunningBetListByBetTypeServiceImpl>(Lifestyle.Singleton);
            container.Register<RunningBetListByBetTypeAndMatchService, RunningBetListByBetTypeAndMatchServiceImpl>(Lifestyle.Singleton);
            container.Register<BullfightingByMatchService, BullfightingByMatchServiceImpl>(Lifestyle.Singleton);
            container.Register<JackpotDetailsService, JackpotDetailsServiceImpl>(Lifestyle.Singleton);

            if (AppSettings.IsIntegrationTest)
            {
                container.RegisterSingleton<IUser, StaticUserSession>();
            }
            else
            {
                container.RegisterSingleton<IUser, User>();
            }
        }

        private static void RegisterStaticInstances(Container container)
        {
            ReportDateServiceImpl.Instance = container.GetInstance<ReportDateService>();
        }
    }
}