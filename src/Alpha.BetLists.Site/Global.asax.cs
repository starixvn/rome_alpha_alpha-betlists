namespace Alpha.BetLists.Site
{
    using System;
    using System.Collections.Generic;
    using System.Configuration;
    using System.Diagnostics;
    using System.Linq;
    using System.Net;
    using System.Reflection;
    using System.Web;
    using System.Web.Helpers;
    using System.Web.Hosting;
    using System.Web.Mvc;
    using System.Web.Routing;
    using Alpha.BetLists.Services;
    using Alpha.NetThrottling.Mvc;
    using Alpha.NetThrottling.Utils;
    using Alpha.PartialUM.Filters;
    using AlphaPlus.Logging;
    using AlphaPlus.Logging.Web;
    using AlphaPlus.User.Models;
    using App_Start;
    using Auth.Filters;
    using Configurations;
    using Fanex.Caching;
    using Fanex.Data;
    using Fanex.Logging;
    using Fanex.Logging.Extensions.Web;
    using Fanex.RemotingRef;
    using Helpers;
    using ONEbook.UM;
    using ONEbook.UM.AspNetMVC.Filters;
    using ONEbook.UM.Configurations;
    using ONEbook.UM.NETFramework;
    using ONEbook.UM.Services;
    using ONEbook.UM.Transport;
    using Web;
    using Web.Filters;
    using Web.Helpers;

    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AlphaPlus.Configurations.ConfigurationManager.Init(new ConfigurationHelper());
            AntiForgeryConfig.SuppressXFrameOptionsHeader = true;

            ViewEngines.Engines.Clear();
            ViewEngines.Engines.Add(new RazorViewEngine());
            MvcHandler.DisableMvcResponseHeader = true;

            AreaRegistration.RegisterAllAreas();
            RouteConfig.RegisterRoutes(RouteTable.Routes);

            var connectionStrings = ConfigurationManager.ConnectionStrings
                .Cast<ConnectionStringSettings>()
                .ToDictionary(
                    connection => connection.Name,
                    connection => new ConnectionConfiguration(connection.Name, connection.ConnectionString, connection.ProviderName),
                    StringComparer.OrdinalIgnoreCase);
            DbSettingProviderManager
                .StartNewSession()
                .Use(connectionStrings)
                .Use(AppSettings.FanexDataServiceApi)
                .Register(resourcePath: AppSettings.StoredProcedurePath)
                .Run();

            DependencyConfig.Init();
            AppSettings.Verify();
            ShareSession.Init();
            LoggingConfig.ExceptionHandler = new ExceptionHandlerAdapter();
            LoggingConfig.Init();

            GlobalFilters.Filters.Add(new NetThrottlingFilterAttribute(false));

            var umConfig = new UMClientConfiguration().Initialize();
            UMClientManager
                .UseConfig(umConfig)
                .UseCacheService(new UMCacheService(new CacheService()))
                .UseRestClient(new UMRestClient(umConfig))
                .UseHttpContext(new ONEbook.UM.Helpers.HttpContextHelper());

            GlobalFilters.Filters.Add(new CheckUMFilterAttribute(
                new MaintenanceService(UMClientManager.RestClient, UMClientManager.CacheService, UMClientManager.UMClientConfigs, UMClientManager.HttpContext), null));
            GlobalFilters.Filters.Add(new CheckBelongingFilter(!AppSettings.IsStandaloneReport));
            GlobalFilters.Filters.Add(new NoCacheFilter());
            GlobalFilters.Filters.Add(new AfterLoginFlowFilter(!AppSettings.IsStandaloneReport));
            GlobalFilters.Filters.Add(new PartialUMExceptionFilter(true));
            GlobalFilters.Filters.Add(new ProtectedModeFilter(!AppSettings.IsStandaloneReport));

            ModelBinders.Binders.DefaultBinder = new QueryEntityModelBinder();

            StaticResources.Init(GetBuildVersion());

            RefDataTypeContainer
                .ConnectTo(new Uri(AppSettings.RemoteRefURL))
                .FromApplication("Alpha.BetLists.Site");

            if (AppSettings.IsEnabledLoggingApplicationStartEnd)
            {
                try
                {
                    var lastBetsService = DependencyResolver.Current.GetService<LastBetsService>();
                    lastBetsService.GetRunningOutrightMatches(AppSettings.RunningMatchCacheTime);
                    lastBetsService.GetRunningOtherMatches(AppSettings.RunningMatchCacheTime);
                }
                finally
                {
                    var error = Server.GetLastError()?.ToString();
                    var hostError = HostingEnvironment.InitializationException?.ToString();
                    var errorMessage = $"{error}{hostError}";
                    errorMessage = string.IsNullOrWhiteSpace(errorMessage) ? string.Empty : $"But then error: {errorMessage}";

                    LogService.Error($"Alpha_Running_Betlist: {Hostname()} started. {errorMessage}");
                }
            }
        }

        protected void Application_End()
        {
            if (AppSettings.IsEnabledLoggingApplicationStartEnd)
            {
                var error = Server.GetLastError()?.ToString();
                var hostError = HostingEnvironment.InitializationException?.ToString();
                var errorMessage = $"{error}{hostError}";
                var shutdownReason = HostingEnvironment.ShutdownReason;

                LogService.Error($"Alpha_Running_Betlist: {Hostname()} ended. Reason: {shutdownReason}. {errorMessage}");
            }
        }

        protected void Application_Error()
        {
            if (Request.IsLocal)
            {
                return;
            }

            Exception ex = Server.GetLastError();
            HttpException httpException = ex as HttpException;
            if (httpException != null && httpException.GetHttpCode() == HttpStatusCodes.TooManyRequests)
            {
                return;
            }

            var currentCustomer = UserSession.Current;
            if (ex is FormatException &&
                Request.Params.AllKeys.Any(key => Auth.Configurations.AuthorizationSection.Instance.CustIdQueryStringParamNames.Any(cf => string.Equals(key, cf, StringComparison.OrdinalIgnoreCase))))
            {
                return;
            }

            ExceptionHandler.Handle(ex,
                new Dictionary<string, object>
                {
                    ["CustId"] = currentCustomer?.CustId,
                    ["CustName"] = currentCustomer?.UserName,
                    ["SubAccountName"] = currentCustomer?.SubUserName,
                    ["AdminName"] = currentCustomer?.AdminName
                });
        }

        private static string GetBuildVersion()
        {
            Assembly assembly = Assembly.GetCallingAssembly();
            FileVersionInfo versionInfo = FileVersionInfo.GetVersionInfo(assembly.Location);

            return $"{versionInfo.FileVersion}.{AppSettings.JsVersion}";
        }

        private static string Hostname()
        {
            return Dns.GetHostName();
        }
    }
}
