namespace Alpha.BetLists.Site
{
    using System.Web.Mvc;
    using System.Web.Routing;
    using Fanex.Data;
    using Fanex.Data.SqlServer;

    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {            
            ViewEngines.Engines.Clear();
            ViewEngines.Engines.Add(new RazorViewEngine());
         
            AreaRegistration.RegisterAllAreas();
            RouteConfig.RegisterRoutes(RouteTable.Routes);

            DbSettingProviderManager
                .StartNewSession()
                .WithSqlServer(resourcePath: "App_Data/SPNames/Local")
                .Run();

            
        }

    }
}
