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
            var userData = _readOnlyRepository.First<Account>(x => x.EMail == User.Identity.Name);

            var emailBody = new StringBuilder("<b>Your friend </b>");
            emailBody.Append("<b>"+userData.Name+" "+userData.LastName+"</b>");
            emailBody.Append("<b> wants you to join to MiniDropBox, a site where you can store your files in the cloud!!</b>");
            emailBody.Append("<br/>");
            emailBody.Append("<b>To register in the site just click on the link below and fill up a quick form! Enjoy!!! </b>");

            //emailBody.Append("http://minidropbox-1.apphb.com/AccountSignUp/AccountSignUp?token=" + userId);
            emailBody.Append("http://localhost:1840/AccountSignUp/AccountSignUp?token=" + userData.Id);
            emailBody.Append("<br/>");
            emailBody.Append("<br/>");
            emailBody.Append("<br/>");

            if (MailSender.SendEmail(model.ReferalEmail,"Join Mini DropBox"  , emailBody.ToString()))
            {
                Success("E-Mail sent successfully!!");
                return View(model);
            }
            Error("E-Mail couldn't be sent!!!!");
            return View(model);
        }
    }
}
