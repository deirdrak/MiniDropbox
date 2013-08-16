using System;
using System.Data;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using BootstrapMvcSample.Controllers;
using MiniDropbox.Domain;
using MiniDropbox.Domain.Services;
using MiniDropbox.Web.Models;
using MiniDropbox.Web.Utils;

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
            return Session["userId"]==null ? RedirectToAction("LogIn", "Account") : RedirectToAction("ListAllContent", "Disk");
        }

        [HttpPost]
        public ActionResult PasswordRecovery(PasswordRecoveryModel model)
        {
            var result = _readOnlyRepository.Query<Account>(a => a.EMail == model.EMailAddress);

            if (result.Any())
            {
                var fechaActual = DateTime.Now.Date;

                var pass = result.FirstOrDefault().Password;
                var data = ""+fechaActual.Day + fechaActual.Month + fechaActual.Year;
                var token =pass+";"+ EncriptacionMD5.Encriptar(data);

                //var url = "http://minidropbox-1.apphb.com/PasswordReset/PasswordReset";
                var url = "http://localhost:1840/PasswordReset/PasswordReset";

                var emailBody = new StringBuilder("<b>Go to the following link to change your password: </b>");
                emailBody.Append("<br/>");
                emailBody.Append("<br/>");
                emailBody.Append("<b>" + url + "?token=" +token + "<b>");
                emailBody.Append("<br/>");
                emailBody.Append("<br/>");
                emailBody.Append("<b>This link is only valid through " + fechaActual.Day + "/" + fechaActual.Month + "/" + fechaActual.Year + "</b>");

                if (MailSender.SendEmail(model.EMailAddress,"Password Recovery" ,emailBody.ToString()))
                    return Cancel();
               
                Error("E-Mail failed to be sent, please try again!!!");
                return View(model);
               
            }

            Error("E-Mail address is not registered in this site!!!");
            return View(model);
        }
    }
}