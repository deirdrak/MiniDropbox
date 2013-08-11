using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BootstrapMvcSample.Controllers;
using MiniDropbox.Web.Models;

namespace MiniDropbox.Web.Controllers
{
    public class AccountProfileController : BootstrapBaseController
    {
        //
        // GET: /AccountProfile/
        [HttpGet]
        public ActionResult Profile()
        {
            return View(new AccountProfileModel());
        }

    }
}
