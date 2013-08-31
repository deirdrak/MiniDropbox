using System;
using System.Web.Mvc;
using AutoMapper;
using BootstrapMvcSample.Controllers;
using MiniDropbox.Domain;
using MiniDropbox.Domain.Services;
using MiniDropbox.Web.Models;

namespace MiniDropbox.Web.Controllers
{
    public class AccountProfileController : BootstrapBaseController
    {
        private readonly IReadOnlyRepository _readOnlyRepository;
        private readonly IWriteOnlyRepository _writeOnlyRepository;

        public AccountProfileController(IReadOnlyRepository readOnlyRepository, IWriteOnlyRepository writeOnlyRepository)
        {
            _readOnlyRepository = readOnlyRepository;
            _writeOnlyRepository = writeOnlyRepository;
        }
       
        [HttpGet]
        public new ActionResult Profile()
        {
            //long userId = Convert.ToInt64(Session["userId"]);
            var userData = _readOnlyRepository.First<Account>(x=>x.EMail==User.Identity.Name);

            return View(Mapper.Map<AccountProfileModel>(userData));
        }

        [HttpPost]
        public ActionResult Profile(AccountProfileModel model)
        {
            //long userId = Convert.ToInt64(Session["userId"]);
            var userData = _readOnlyRepository.First<Account>(x => x.EMail == User.Identity.Name);

            userData.Name = model.Name;
            userData.LastName = model.LastName;

            _writeOnlyRepository.Update(userData);

            return RedirectToAction("ListAllContent", "Disk");
        }

        public ActionResult Cancel()
        {
            return RedirectToAction("ListAllContent", "Disk");
        }

        public ActionResult ChangePassword()
        {
            return RedirectToAction("ChangePassword", "ChangePassword");
        }
    }
}
