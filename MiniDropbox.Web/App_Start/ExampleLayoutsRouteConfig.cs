using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using BootstrapMvcSample.Controllers;
using MiniDropbox.Web.Controllers;
using NavigationRoutes;

namespace BootstrapMvcSample
{
    public class ExampleLayoutsRouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            //routes.MapNavigationRoute<HomeController>("Automatic Scaffolding", c => c.Index());

            //routes.MapNavigationRoute<ExampleLayoutsController>("Example Layouts", c => c.Starter())
            //      .AddChildRoute<ExampleLayoutsController>("Marketing", c => c.Marketing())
            //      .AddChildRoute<ExampleLayoutsController>("Fluid", c => c.Fluid())
            //      .AddChildRoute<ExampleLayoutsController>("Sign In", c => c.SignIn())
            //    ;
            routes.MapNavigationRoute<ExampleLayoutsController>("Options", c => c.Starter())
                .AddChildRoute<AccountProfileController>("View Profile", c => c.Profile())
                .AddChildRoute<DiskController>("My Files", c => c.ListAllContentRoot())
                .AddChildRoute<ChangePasswordController>("Change Password",p=>p.ChangePassword())
                .AddChildRoute<ReferralInviteController>("Refer a friend", c => c.ReferralInvite())
                .AddChildRoute<AccountController>("Log Out",a=>a.LogOut());

            
        }
    }
}
