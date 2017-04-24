namespace Alpha.BetLists.Site.App_Start
{
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Reflection;
    using System.Web.Mvc;
    using BetLists.Models;
    using Controllers;
    using DataAccesses;
    using Fanex.BetList.Core.Builder;
    using Fanex.BetList.Core.Entities;
    using Fanex.BetList.Core.Factories;
    using Fanex.BetList.Core.TicketHelper;
    using Fanex.Caching;
    using Fanex.Data.Repository;
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
    using Sunplus.Agent.Model.Base;
    using Sunplus.Agent.Model.Entities;
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
        }

        private static void RegisterServices(Container container)
        {
#pragma warning disable CS0618 // Type or member is obsolete
            container.RegisterSingleton<IUserSession, UserSessionExt>();
#pragma warning restore CS0618 // Type or member is obsolete
            container.RegisterSingleton<IUser, User>();
            container.Register<WinLossService, WinLossServiceImpl>(Lifestyle.Singleton);
            container.Register<ReportDateService, ReportDateServiceImpl>(Lifestyle.Singleton);
            container.Register<ParlayService, ParlayServiceImpl>(Lifestyle.Singleton);
            container.Register<ISmartPunterService, SmartPunterService>(Lifestyle.Singleton);
#pragma warning disable CS0618 // Type or member is obsolete
            container.Register(
                () => SmartPunterBetListMonitor.CreateInstance(
                    container.GetInstance<IUserSession>(),
                    container.GetInstance<ISmartPunterService>(),
                    IPHelper.CustomerIP),
                Lifestyle.Singleton);
#pragma warning restore CS0618 // Type or member is obsolete

            container.Register<IPService, IPServiceImpl>(Lifestyle.Singleton);

            container.Register<WinLossByMatchService, WinLossByMatchServiceImpl>(Lifestyle.Singleton);
        }
    }
}