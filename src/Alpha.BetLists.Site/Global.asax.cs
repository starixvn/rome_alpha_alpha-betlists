namespace Alpha.BetLists.Site
{
    using System.Web.Mvc;
    using System.Web.Routing;
    using App_Start;
    using Auth.Web;
    using Sunplus.Agent.Utilities;

    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            RouteConfig.RegisterRoutes(RouteTable.Routes);

            DependencyConfig.Init();

            Fanex.Data.Configuration.DbSettingProviderManager.Current.Start();

            ShareSession.Init();

            Logs.WriteInfo("Alpha - Bet Lists - Application Start");
        }
    }
}