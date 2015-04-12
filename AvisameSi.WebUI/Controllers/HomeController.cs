using AvisameSi.Redis.Infrastructure;
using AvisameSi.ServiceLibrary;
using AvisameSi.ServiceLibrary.Entities;
using AvisameSi.ServiceLibrary.Implementations;
using AvisameSi.ServiceLibrary.RespositoryContracts;
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

            IPostRepository postRepository = new PostRepository(MvcApplication.RedisConn);
            PostService service = new PostService(postRepository);
            IEnumerable<Post> model =  service.GetGlobalTimeline(1, 100);
            return View(model);
        }

        [HttpPost]
        public ActionResult Index(string message)
        {

            IPostRepository postRepository = new PostRepository(MvcApplication.RedisConn);
            PostService service = new PostService(postRepository);
            service.SavePost(
            new Post()
            {
                Email = User.Identity.Name,
                Time = DateTime.Now,
                Message = message
            });
            IEnumerable<Post> model = service.GetGlobalTimeline(0, 100);
            return View(model);
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