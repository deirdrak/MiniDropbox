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
    public class RegisteredUsersListController : BootstrapBaseController
    {
        private readonly IReadOnlyRepository _readOnlyRepository;
        private readonly IWriteOnlyRepository _writeOnlyRepository;

        public RegisteredUsersListController(IReadOnlyRepository readOnlyRepository, IWriteOnlyRepository writeOnlyRepository)
        {
            _readOnlyRepository = readOnlyRepository;
            _writeOnlyRepository = writeOnlyRepository;
        }
        
        [HttpGet]
        public ActionResult RegisteredUsersList()
        {
            if (Session["userType"].ToString() != "Admin")
            {
                return null;
            }

            var usersList = _readOnlyRepository.GetAll<Account>();
            var castedList = new List<RegisteredUsersListModel>();

            foreach (var account in usersList)
            {
                castedList.Add(new RegisteredUsersListModel(account.Id, account.IsArchived, account.Name,
                    account.LastName, account.EMail, account.IsBlocked, account.SpaceLimit));
            }

            return View(castedList);
        }
        
        public ActionResult BlockUser(long userId)
        {
            var userData = _readOnlyRepository.GetById<Account>(userId);

            userData.IsBlocked = !userData.IsBlocked;

            _writeOnlyRepository.Update(userData);

            return RedirectToAction("RegisteredUsersList");
        }

        public ActionResult PackageManagement()
        {
            if (Session["userType"].ToString() != "Admin")
            {
                return null;
            }

            return RedirectToAction("PackageList", "PackageList");
        }

    }
}
