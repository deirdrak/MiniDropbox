
using System;
using System.Linq;
using System.Web.Mvc;
using BootstrapMvcSample.Controllers;
using BootstrapSupport;
using FluentNHibernate.Conventions;
using MiniDropbox.Domain;
using MiniDropbox.Domain.Services;
using MiniDropbox.Web.Models;

namespace MiniDropbox.Web.Controllers
{
    public class PasswordResetController : BootstrapBaseController
    {
        private readonly IReadOnlyRepository _readOnlyRepository;
        private readonly IWriteOnlyRepository _writeOnlyRepository;

        public PasswordResetController(IWriteOnlyRepository writeOnlyRepository, IReadOnlyRepository readOnlyRepository)
        {
            _readOnlyRepository = readOnlyRepository;
            _writeOnlyRepository = writeOnlyRepository;
        }

        [HttpGet]
        public ActionResult PasswordReset(string token)
        {
            if (token == "ErrorPostback")
            {
                Error("Link has expired!!!");
                return View();
            }
           
            var fechaActual = DateTime.Now.Date;

            var data = token.Split(';');
            var password = data[0];
            var linkDate = data[1];

            var currentDate = "" + fechaActual.Day + fechaActual.Month + fechaActual.Year;
            var currentDateMd5 = EncriptacionMD5.Encriptar(currentDate);

            var user = _readOnlyRepository.Query<Account>(a => a.Password == password);

            if (linkDate == currentDateMd5 && user.Any())
            {
                return View(new PasswordResetModel { UserId = user.FirstOrDefault().Id });
            }

            return RedirectToAction("PasswordReset", new { token = "ErrorPostBack" });
           
        }

        [HttpPost]
        public ActionResult PasswordReset(PasswordResetModel model)
        {
            var newPassword = EncriptacionMD5.Encriptar(model.Password);
            var user = _readOnlyRepository.GetById<Account>(model.UserId);

            user.Password = newPassword;
            _writeOnlyRepository.Update<Account>(user);

            return RedirectToAction("LogIn", "Account");
        }

    }
    
}
