using AvisameSi.Redis.Infrastructure;
using AvisameSi.ServiceLibrary.Implementations;
using AvisameSi.ServiceLibrary.RespositoryContracts;
using AvisameSi.WebUI.Security;
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
            _rediConn = ConnectionMultiplexer.Connect("localhost");
            IAccountRepository accountRepository = new AccountRepository(_rediConn);
            AccountService service = new AccountService(accountRepository);
            GlobalFilters.Filters.Add(new AvisameSiAuthenticateAttribute(service));
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            ViewEngines.Engines.Clear();
            ViewEngines.Engines.Add(new RazorViewEngine());



           
        }
    }
}
