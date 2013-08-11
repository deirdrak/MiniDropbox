using System;
using System.Linq;
using System.Web.Mvc;
using System.Web.UI.WebControls;
using BootstrapMvcSample.Controllers;
using MiniDropbox.Domain;
using MiniDropbox.Domain.Services;
using MiniDropbox.Web.Models;

namespace MiniDropbox.Web.Controllers
{
    public class PasswordRecoveryController : BootstrapBaseController
    {
        private readonly IReadOnlyRepository _readOnlyRepository;

        public PasswordRecoveryController(IReadOnlyRepository readOnlyRepository)
        {
            _readOnlyRepository = readOnlyRepository;
        }

        [HttpGet]
        public ActionResult PasswordRecovery()
        {
            return View(new PasswordRecoveryModel());
        }

        public ActionResult Cancel()
        {
            return RedirectToAction("LogIn", "Account");
        }

        [HttpPost]
        public ActionResult PasswordRecovery(PasswordRecoveryModel model)
        {
            var result = _readOnlyRepository.Query<Account>(a => a.EMail == model.EMailAddress);

            if (result.Any())
            {
                var nameMail = result.FirstOrDefault().Name + result.FirstOrDefault().EMail+DateTime.Now.Date;
                var token = EncriptacionMD5.Encriptar(nameMail);
                var url = "";
                var emailBody = "Go to the following link to change your password: "+url+"token"+token;
                //Send the email
                return Cancel();
            }

            Error("E-Mail address is not registered in this site!!!");
            return View(model);
        }
    }
}