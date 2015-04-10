using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace AvisameSi.WebUI
{
    public class MvcApplication : System.Web.HttpApplication
    {
        private static ConnectionMultiplexer _rediConn;
        public static ConnectionMultiplexer RedisConn 
        {
            get
            {
                return _rediConn;
            }
        }

        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            ViewEngines.Engines.Clear();
            ViewEngines.Engines.Add(new RazorViewEngine());

            _rediConn = ConnectionMultiplexer.Connect("localhost");
        }
    }
}
