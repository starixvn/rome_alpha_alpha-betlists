namespace Alpha.BetLists.Site
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;
    using System.Reflection;
    using System.Web;
    using System.Web.Helpers;
    using System.Web.Mvc;
    using System.Web.Routing;
    using Alpha.Logging;
    using Alpha.NetThrottling.Mvc;
    using Alpha.NetThrottling.Utils;
    using Alpha.PartialUM.Filters;
    using App_Start;
    using Auth.Filters;
    using Configurations;
    using Fanex.Caching;
    using Fanex.Data;
    using Fanex.Data.CrossCutting.WrappingException;
    using Fanex.Data.OldConnectionStringProvider;
    using Fanex.Logging.Extensions.Database;
    using Fanex.Logging.Extensions.Web;
    using Fanex.RemotingRef;
    using Helpers;
    using ONEbook.UM;
    using ONEbook.UM.AspNetMVC.Filters;
    using ONEbook.UM.Configurations;
    using ONEbook.UM.Helpers;
    using ONEbook.UM.NETFramework;
    using ONEbook.UM.Services;
    using ONEbook.UM.Transport;
    using Sunplus.Agent.Model.Entities;
    using Web;
    using Web.Filters;
    using Web.Helpers;

    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AntiForgeryConfig.SuppressXFrameOptionsHeader = true;

            ViewEngines.Engines.Clear();
            ViewEngines.Engines.Add(new RazorViewEngine());
            MvcHandler.DisableMvcResponseHeader = true;

            AreaRegistration.RegisterAllAreas();
            RouteConfig.RegisterRoutes(RouteTable.Routes);

            DependencyConfig.Init();

            AppSettings.Verify();

            DbSettingProviderManager
                .StartNewSession()
                .Use(new OldConnectionStringProvider())
                .UseDefaultDbSettingProvider(resourcePath: AppSettings.StoredProcedurePath, enableWatching: true)
                .Use((settingKey, commandTimeout) => new WrappingExceptionObjectDb(settingKey, commandTimeout))
                .Run();

            ShareSession.Init();

            LoggingConfig.Init(getDbExceptionInfo: DatabaseInfoGetters.GetDbExceptionInfo);
            if (!AppSettings.IsIntegrationTest)
            {
                GlobalFilters.Filters.Add(new NetThrottlingFilterAttribute(false));

                var umConfig = new UMClientConfiguration().Initialize();
                UMClientManager
                    .UseConfig(umConfig)
                    .UseCacheService(new UMCacheService(new CacheService()))
                    .UseRestClient(new UMRestClient(umConfig))
                    .UseHttpContext(new HttpContextHelper());

                GlobalFilters.Filters.Add(new CheckUMFilterAttribute(
                    new MaintenanceService(UMClientManager.RestClient, UMClientManager.CacheService, UMClientManager.UMClientConfigs, UMClientManager.HttpContext), null));

                GlobalFilters.Filters.Add(new CheckBelongingFilter(!AppSettings.IsStandaloneReport));
                GlobalFilters.Filters.Add(new NoCacheFilter());
                GlobalFilters.Filters.Add(new AfterLoginFlowFilter(!AppSettings.IsStandaloneReport));
                GlobalFilters.Filters.Add(new PartialUMExceptionFilter(true));
                GlobalFilters.Filters.Add(new ProtectedModeFilter(!AppSettings.IsStandaloneReport));
            }

            ModelBinders.Binders.DefaultBinder = new QueryEntityModelBinder();

            StaticResources.Init(AppSettings.IsIntegrationTest ? string.Empty : GetBuildVersion());

            RefDataTypeContainer
                .ConnectTo(new Uri(AppSettings.RemoteRefURL))
                .FromApplication("Alpha.BetLists.Site");
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

            return versionInfo.FileVersion;
        }
    }
}
