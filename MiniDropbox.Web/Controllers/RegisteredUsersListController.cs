using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using AutoMapper;
using BootstrapMvcSample.Controllers;
using MiniDropbox.Domain;
using MiniDropbox.Domain.Services;
using MiniDropbox.Web.Models;
using MiniDropbox.Web.Utils;
using Ninject.Infrastructure.Language;

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
            //if (Session["userType"].ToString() != "Admin")
            //{
            //    return null;
            //}

            var usersList = _readOnlyRepository.GetAll<Account>();
            
            var castedList = new List<RegisteredUsersListModel>();

            foreach (var account in usersList)
            {
                castedList.Add(Mapper.Map<RegisteredUsersListModel>(account));
            }

            return View(castedList);
        }
        
        public ActionResult BlockUser(long userId)
        {
            var userData = _readOnlyRepository.GetById<Account>(userId);

            userData.IsBlocked = !userData.IsBlocked;

            if (userData.IsBlocked)
            {
                var emailBody =
                    new StringBuilder(
                        "<p><b>Your account has been blocked by the site administrator due to policy and/or terms of usage violation. </b></p>");
                emailBody.Append(
                    "<p><b>If you want more information about the reasons of this decision contact the site admin at admin@minidropbox.com </b></p>");

                MailSender.SendEmail(userData.EMail, "Blocked Account", emailBody.ToString());

            }
            else
            {
                var emailBody =
                    new StringBuilder(
                        "<p><b>Your account has been unblocked by the site administrator you can now log in to Mini DropBox again. </b></p>");
                emailBody.Append(
                    "<p><b>Enjoy your privileges as a registered user now!!! ;) </b></p>");

                MailSender.SendEmail(userData.EMail, "Unblocked Account", emailBody.ToString());
            }

            _writeOnlyRepository.Update(userData);

            return RedirectToAction("RegisteredUsersList");
        }

        public ActionResult PackageManagement()
        {
            //if (Session["userType"].ToString() != "Admin")
            //{
            //    return null;
            //}

            return RedirectToAction("PackageList", "PackageList");
        }

    }
}
