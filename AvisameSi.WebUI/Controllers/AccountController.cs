using AvisameSi.Redis.Infrastructure;
using AvisameSi.ServiceLibrary.Implementations;
using AvisameSi.ServiceLibrary.RespositoryContracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AvisameSi.WebUI.Controllers
{
    public class AccountController : Controller
    {
        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(string email, string password)
        {
            IAccountRepository accountRepository = new AccountRepository(MvcApplication.RedisConn);
            AvisameSiService service = new AvisameSiService(accountRepository);
            service.Login(email, password);
            return RedirectToAction("Index", "Home"); 
        }

        [HttpGet]
        public ActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Register(string email, string password, string passwordRepeat)
        {
            try
            {
                IAccountRepository accountRepository = new AccountRepository(MvcApplication.RedisConn);
                AvisameSiService service = new AvisameSiService(accountRepository);
                service.Register(email, password);
                return RedirectToAction("Index", "Home");
            }
            catch
            {
                return RedirectToAction("Error500", "Error");
            }
        }
    }
}