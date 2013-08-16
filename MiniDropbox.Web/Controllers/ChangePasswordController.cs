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
    public class ChangePasswordController : BootstrapBaseController
    {
        private readonly IReadOnlyRepository _readOnlyRepository;
        private readonly IWriteOnlyRepository _writeOnlyRepository;

        public ChangePasswordController(IReadOnlyRepository readOnlyRepository, IWriteOnlyRepository writeOnlyRepository)
        {
            _readOnlyRepository = readOnlyRepository;
            _writeOnlyRepository = writeOnlyRepository;
        }

        [HttpGet]
        public ActionResult ChangePassword()
        {
            return View(new ChangePasswordModel());
        }

        [HttpPost]
        public ActionResult ChangePassword(ChangePasswordModel model)
        {
            var userId =Convert.ToInt64(Session["userId"]);
            var userData = _readOnlyRepository.GetById<Account>(userId);

            var oldPasswordEncripted = EncriptacionMD5.Encriptar(model.OldPassword);

            if (userData.Password != oldPasswordEncripted)
            {
                Error("The old password is incorrect!!!");
                
                ClearModel(model);

                return View(model);
            }

            userData.Password = EncriptacionMD5.Encriptar(model.NewPassword);
            _writeOnlyRepository.Update(userData);

            Success("Password changed successfully!!");

            ClearModel(model);

            return View(model);
        }

        private void ClearModel(ChangePasswordModel model)
        {
            model.OldPassword = string.Empty;
            model.NewPassword = string.Empty;
            model.ConfirmPassword = string.Empty;
        }

        public ActionResult Cancel()
        {
            return RedirectToAction("Profile","AccountProfile");
        }
    }
}
