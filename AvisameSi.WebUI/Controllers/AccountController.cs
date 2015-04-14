using AvisameSi.Redis.Infrastructure;
using AvisameSi.ServiceLibrary.Entities;
using AvisameSi.ServiceLibrary.Implementations;
using AvisameSi.ServiceLibrary.RespositoryContracts;
using AvisameSi.WebUI.Security;
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
        [AllowAnonymous]
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        public ActionResult Login(string email, string password)
        {
            IAccountRepository accountRepository = new AccountRepository(MvcApplication.RedisConn);
            AccountService service = new AccountService(accountRepository);
            UserEntity user = new UserEntity()
            {
                Email = email,
                Password = password
            };
            UserTokenEntity token = service.Login(user);
            if (token != null)
            {
                AuthCookieHelper.CreateAuthCookie(email, token.GetTokenString(), false);
                return RedirectToAction("Index", "Home"); 
            }
            else
            {
                return RedirectToAction("Register"); 
            }
        }

        [HttpGet]
        [AllowAnonymous]
        public ActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        public ActionResult Register(string email, string password, string passwordRepeat)
        {
            try
            {
                IAccountRepository accountRepository = new AccountRepository(MvcApplication.RedisConn);
                AccountService service = new AccountService(accountRepository);
                
                UserEntity user = new UserEntity()
                {
                    Email = email,
                    Password = password
                };

                UserTokenEntity token = service.Register(user);
                AuthCookieHelper.CreateAuthCookie(email, token.GetTokenString(), false);

                return RedirectToAction("Index", "Home");
            }
            catch
            {
                throw;
                //return RedirectToAction("Error500", "Error");
            }
        }
    }
}