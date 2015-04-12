using AvisameSi.ServiceLibrary.Implementations;
using System;
using System.Security.Claims;
using System.Security.Principal;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.Security;

namespace AvisameSi.WebUI.Security
{
    public class AvisameSiAuthenticateAttribute : AuthorizeAttribute
    {
        private readonly AccountService _avisameSiService;

        public AvisameSiAuthenticateAttribute(AccountService avisameSiService)
        {
            _avisameSiService = avisameSiService;
        }

        public override void OnAuthorization(AuthorizationContext filterContext)
        {
            if (filterContext.ActionDescriptor.GetCustomAttributes(typeof(AllowAnonymousAttribute), true).Length != 0)
            {
                return;
            }

            HttpCookie cookie = HttpContext.Current.Request.Cookies[AuthCookieHelper.AuthCookieName];
            if (cookie != null)
            {
                FormsAuthenticationTicket ticket = FormsAuthentication.Decrypt(cookie.Value);
                string email = ticket.Name;
                string token = ticket.UserData;

                if (_avisameSiService.IsUserLogged(email, token))
                {
                    var identity = new GenericIdentity(email, "AvisameSi");
                    //identity.AddClaim(new Claim("userEmail", ticket.Name));

                    HttpContext.Current.User = new GenericPrincipal(identity, null);
                    return;
                } 
            }
            
            this.HandleUnauthorizedRequest(filterContext);
        }
            
        protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
        {
            filterContext.Result = new RedirectToRouteResult(new RouteValueDictionary(new { controller = "Account", action = "Login" }));
        }

        
    }
}