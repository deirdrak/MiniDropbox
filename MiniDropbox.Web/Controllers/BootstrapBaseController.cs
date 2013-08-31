using System;
using System.Collections.Generic;
using System.Configuration;
using System.Web;
using System.Web.Configuration;
using System.Web.Mvc;
using System.Web.Security;
using BootstrapSupport;

namespace BootstrapMvcSample.Controllers
{
    public class BootstrapBaseController: Controller
    {
        public void Attention(string message)
        {
            TempData.Add(Alerts.ATTENTION, message);
        }

        public void Success(string message)
        {
            TempData.Add(Alerts.SUCCESS, message);
        }

        public void Information(string message)
        {
            TempData.Add(Alerts.INFORMATION, message);
        }

        public void Error(string message)
        {
            TempData.Add(Alerts.ERROR, message);
        }

        public void SetAuthenticationCookie(string userEmail, List<string> roles)
        {
            var cookieSection = (HttpCookiesSection)ConfigurationManager.GetSection("system.web/httpCookies");

            var authenticationSection =
                (AuthenticationSection)ConfigurationManager.GetSection("system.web/authentication");


            var authTicket =
                new FormsAuthenticationTicket(
                    1, userEmail, DateTime.Now,
                    DateTime.Now.AddMinutes(authenticationSection.Forms.Timeout.TotalMinutes),
                    false, string.Join("|", roles.ToArray()));


            String encryptedTicket = FormsAuthentication.Encrypt(authTicket);

            var authCookie = new HttpCookie(FormsAuthentication.FormsCookieName, encryptedTicket);


            if (cookieSection.RequireSSL || authenticationSection.Forms.RequireSSL)
            {
                authCookie.Secure = true;
            }


            HttpContext.Response.Cookies.Add(authCookie);

        }
    }
}
