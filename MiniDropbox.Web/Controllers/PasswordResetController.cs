using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BootstrapMvcSample.Controllers;
using MiniDropbox.Web.Models;

namespace MiniDropbox.Web.Controllers
{
    public class PasswordResetController : BootstrapBaseController
    {
        //
        // GET: /PasswordReset/
        [HttpGet]
        public ActionResult PasswordReset(string token)
        {
            return View(new PasswordResetModel());
        }

    }
    
}
