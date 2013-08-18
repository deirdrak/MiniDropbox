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
    public class ChangeUserSpaceLimitController : BootstrapBaseController
    {
        private readonly IReadOnlyRepository _readOnlyRepository;
        private readonly IWriteOnlyRepository _writeOnlyRepository;

        public ChangeUserSpaceLimitController(IWriteOnlyRepository writeOnlyRepository, IReadOnlyRepository readOnlyRepository)
        {
            _readOnlyRepository = readOnlyRepository;
            _writeOnlyRepository = writeOnlyRepository;
        }
        //
        // GET: /ChangeUserSpaceLimit/
        [HttpGet]
        public ActionResult ChangeUserSpaceLimit(long userId)
        {
            if (Session["userType"].ToString() != "Admin")
            {
                return null;
            }

            var userData = _readOnlyRepository.GetById<Account>(userId);

            return View(new ChangeUserSpaceLimitModel()
            {
                UserId = userData.Id,
                Name = userData.Name,
                LastName = userData.LastName,
                EMail = userData.EMail,
                SpaceLimit = userData.SpaceLimit,
                Archived = userData.IsArchived,
                Blocked = userData.IsBlocked
            });
        }

        [HttpPost]
        public ActionResult ChangeUserSpaceLimit(ChangeUserSpaceLimitModel model)
        {
            var userData = _readOnlyRepository.GetById<Account>(model.UserId);

            if (model.SpaceLimit < userData.UsedSpace)
            {
                Error("The space limit can't be less than the space currently used by the User!!!");
                return View(model);
            }

            var emailBody =new StringBuilder();

            if (model.SpaceLimit > userData.SpaceLimit)
            {
                emailBody.Append("<p><b>Your space limit has been increased!!!</b></p><p><b>You now have </b></p>");
                emailBody.Append("<b>" + model.SpaceLimit + "<b>");
                emailBody.Append("<b> available in your drive!!!!</b>");
            }
            else
            {
                emailBody.Append("<p><b>Your space limit has been decreased!!!</b></p><p><b>You now have </b></p>");
                emailBody.Append("<b>" + model.SpaceLimit + "<b>");
                emailBody.Append("<b> available in your drive!!!!</b>");
            }

            userData.SpaceLimit = model.SpaceLimit;

            _writeOnlyRepository.Update(userData);
               
            emailBody.Append(
                "<p><b>If you want additional information about the reasons of this decision contact the site admin at admin@minidropbox.com </b></p>");

            MailSender.SendEmail(userData.EMail, "Space Limit Change", emailBody.ToString());


            return RedirectToAction("RegisteredUsersList", "RegisteredUsersList");
        }

        public ActionResult Cancel()
        {
            return RedirectToAction("RegisteredUsersList", "RegisteredUsersList");
        }

    }
}
