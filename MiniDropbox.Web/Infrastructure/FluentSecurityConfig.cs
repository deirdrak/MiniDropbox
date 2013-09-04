using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Security;
using BootstrapMvcSample.Controllers;
using FluentSecurity;
using MiniDropbox.Web.Controllers;

namespace MiniDropbox.Web.Infrastructure
{
    public static class SecurityHelpers
        {
            public static IEnumerable<object> UserRoles()
            {
                var listRoles = new List<string> {"Admin", "User"};
                return listRoles;
            }
        }


    public static class FluentSecurityConfig
    {
        public static void Configure()
        {
            SecurityConfigurator.Configure(configuration =>
            {
                configuration.GetAuthenticationStatusFrom(() => HttpContext.Current.User.Identity.IsAuthenticated);
                configuration.GetRolesFrom(SecurityHelpers.UserRoles);

                configuration.ForAllControllers().DenyAnonymousAccess();
                configuration.For<AccountController>().Ignore();
                configuration.For<AccountSignUpController>().Ignore();
                configuration.For<PasswordRecoveryController>().Ignore();
                configuration.For<PasswordResetController>().Ignore();
                //configuration.For<DiskController>().RequireRole(new object[] { "User" });

                configuration.For<RegisteredUsersListController>().RequireRole(new object[] { "Admin" });
                configuration.For<PackageListController>().RequireRole(new object[] { "Admin" });
                configuration.For<CreateEditPackageController>().RequireRole(new object[] { "Admin" });
                configuration.For<ChangeUserSpaceLimitController>().RequireRole(new object[] { "Admin" });

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