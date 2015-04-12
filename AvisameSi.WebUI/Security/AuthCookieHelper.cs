using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;

namespace AvisameSi.WebUI.Security
{
    public static class AuthCookieHelper
    {
        public const string AuthCookieName = "avisamesi-auth";
        public const string Token = "token";
        public const string Email = "email";

        public static void CreateAuthCookie(string email, string token, bool isPersistent)
        {
            FormsAuthenticationTicket authTicket = new FormsAuthenticationTicket(1,
                                                                                email,
                                                                                DateTime.Now,
                                                                                DateTime.Now.AddMinutes(30),
                                                                                isPersistent,
                                                                                token);

             
            string encryptedTicket = FormsAuthentication.Encrypt(authTicket);
            System.Web.HttpCookie authCookie = new System.Web.HttpCookie(AuthCookieName, encryptedTicket);
            if (authTicket.IsPersistent)
            {
                authCookie.Expires = authTicket.Expiration;
            }
            System.Web.HttpContext.Current.Response.Cookies.Add(authCookie); 
        }

        public static void DeleteAuthCookie(string username)
        {
            var cookie = HttpContext.Current.Request.Cookies[AuthCookieName];
            cookie.Expires = DateTime.Now.AddMinutes(-1);
            HttpContext.Current.Response.Cookies.Add(cookie);
        }
    }
}