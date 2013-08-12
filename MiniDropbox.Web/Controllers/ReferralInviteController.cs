using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using BootstrapMvcSample.Controllers;
using MiniDropbox.Domain;
using MiniDropbox.Domain.Services;
using MiniDropbox.Web.Models;
using MiniDropbox.Web.Utils;

namespace MiniDropbox.Web.Controllers
{
    public class ReferralInviteController : BootstrapBaseController
    {
        private readonly IReadOnlyRepository _readOnlyRepository;
        private readonly IWriteOnlyRepository _writeOnlyRepository;

        public ReferralInviteController(IReadOnlyRepository readOnlyRepository, IWriteOnlyRepository writeOnlyRepository)
        {
            _readOnlyRepository = readOnlyRepository;
            _writeOnlyRepository = writeOnlyRepository;
        }
        //
        // GET: /ReferalInvite/
        [HttpGet]
        public ActionResult ReferralInvite()
        {
            return View(new ReferralInviteModel());
        }

        public ActionResult Cancel()
        {
            return RedirectToAction("ListAllContent", "Disk");
        }

        [HttpPost]
        public ActionResult ReferralInvite(ReferralInviteModel model)
        {
            var userId = Convert.ToInt64(Session["userId"]);
            var userData = _readOnlyRepository.GetById<Account>(userId);

            var emailBody = new StringBuilder("<b>Your friend </b>");
            emailBody.Append(userData.Name+" "+userData.LastName);
            emailBody.Append("<b> wants you to join to MiniDropBox, a site where you can store your files in the cloud!!</b>");
            emailBody.Append("<br/>");
            emailBody.Append("<b>To register in the site just click on the link below and fill up a quick form! Enjoy!!! </b>");

            emailBody.Append("http://minidropbox-1.apphb.com/AccountSignUp/AccountSignUp?token=" + userId);
            emailBody.Append("<br/>");
            emailBody.Append("<br/>");
            emailBody.Append("<br/>");

            if (MailSender.SendEmail(model.ReferalEmail, emailBody.ToString()))
            {
                Success("E-Mail sent successfully!!");
                return View(model);
            }
            Error("E-Mail couldn't be sent!!!!");
            return View(model);
        }
    }
}
