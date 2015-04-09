using AvisameSi.ServiceLibrary;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AvisameSi.WebUI.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            ConnectionMultiplexer redisConn = ConnectionMultiplexer.Connect("localhost");
            AvisameSiService service = new AvisameSiService(redisConn);
            service.Register("jorge", "test");
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}