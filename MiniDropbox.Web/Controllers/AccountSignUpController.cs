using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Services.Description;
using AutoMapper;
using BootstrapMvcSample.Controllers;
using MiniDropbox.Domain;
using MiniDropbox.Domain.Services;
using MiniDropbox.Web.Models;


namespace MiniDropbox.Web.Controllers
{
    public class AccountSignUpController : BootstrapBaseController
    {
        private readonly IReadOnlyRepository _readOnlyRepository;
        private readonly IWriteOnlyRepository _writeOnlyRepository;

        public AccountSignUpController( IWriteOnlyRepository writeOnlyRepository, IReadOnlyRepository readOnlyRepository)
        {
            _readOnlyRepository = readOnlyRepository;
            _writeOnlyRepository = writeOnlyRepository;
        }
        
        [HttpGet]
        public ActionResult AccountSignUp()
        {
            return View(new AccountSignUpModel(500,0,false,false));
        }
       
        public ActionResult Cancelar()
        {
            return RedirectToAction("LogIn","Account");
        }

        [HttpPost]
        public ActionResult AccountSignUp(AccountSignUpModel model)
        {
            var result = _readOnlyRepository.Query<Account>(a=>a.EMail==model.EMail);

            if (result.Any())
            {
                Error("Email account is already registered in this site!!!");
                return View(model);
            }

            var accountModel = Mapper.Map<AccountSignUpModel>(model);

            var account = new Account
            {
                Name = accountModel.Name,
                LastName = accountModel.LastName,
                EMail = accountModel.EMail,
                IsArchived = false,
                Password = EncriptacionMD5.Encriptar(accountModel.Password) 
            };

            _writeOnlyRepository.Create(account);
            return Cancelar();
        }

    }
}
