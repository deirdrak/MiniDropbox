using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using BootstrapMvcSample.Controllers;
using FluentSecurity;
using MiniDropbox.Web.Controllers;

namespace MiniDropbox.Web.Infrastructure
{
    public static class FluentSecurityConfig
    {
        public static void Configure()
        {
            SecurityConfigurator.Configure(configuration =>
            {
                configuration.GetAuthenticationStatusFrom(() => HttpContext.Current.User.Identity.IsAuthenticated);

                configuration.ForAllControllers().DenyAnonymousAccess();
                configuration.For<AccountController>(x => x.LogIn()).Ignore();
                configuration.For<AccountSignUpController>(x => x.AccountSignUp()).Ignore();
                configuration.For<HomeController>(x => x.Create()).RequireRole(new object[] {"Admin"});
                configuration.ResolveServicesUsing(type =>
                {
                    if (type == typeof(IPolicyViolationHandler))
                    {
                        var types = Assembly
                            .GetAssembly(typeof(MvcApplication))
                            .GetTypes()
                            .Where(x => typeof(IPolicyViolationHandler).IsAssignableFrom(x)).ToList();

                        var handlers = types.Select(t => Activator.CreateInstance(t) as IPolicyViolationHandler).ToList();

                        return handlers;
                    }
                    return Enumerable.Empty<object>();
                });
            });

        }
    }
}