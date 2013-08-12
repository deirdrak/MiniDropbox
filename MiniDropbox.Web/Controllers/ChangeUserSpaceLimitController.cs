using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BootstrapMvcSample.Controllers;
using MiniDropbox.Domain;
using MiniDropbox.Domain.Services;
using MiniDropbox.Web.Models;

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

            userData.SpaceLimit = model.SpaceLimit;

            _writeOnlyRepository.Update(userData);

            return RedirectToAction("RegisteredUsersList", "RegisteredUsersList");
        }

        public ActionResult Cancel()
        {
            return RedirectToAction("RegisteredUsersList", "RegisteredUsersList");
        }

    }
}
