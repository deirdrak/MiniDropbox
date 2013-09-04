using System.Web.Mvc;
using System.Web.Routing;
using FluentSecurity;

namespace MiniDropbox.Web.Infrastructure
{
    public class RequireRolePolicyViolationHandler : IPolicyViolationHandler
    {
            public ActionResult Handle(PolicyViolationException exception)
            {
                return new RedirectToRouteResult(new RouteValueDictionary(new { controller = "Account", action = "LogIn", area = "" }));
            }
    }
}