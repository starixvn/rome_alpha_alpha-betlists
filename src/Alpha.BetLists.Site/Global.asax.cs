namespace Alpha.BetLists.Site
{
    using Fanex.Data;
    using System;
    using System.Configuration;
    using System.Linq;
    using System.Web.Mvc;
    using System.Web.Routing;

    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {            
            ViewEngines.Engines.Clear();
            ViewEngines.Engines.Add(new RazorViewEngine());
         
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
                .Use("http://fanexdataservices.nexdev.net")
                .Register(resourcePath: "App_Data/SPNames/Local")
                .Run();

            
        }

    }
}
