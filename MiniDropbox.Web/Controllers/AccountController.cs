using System.Linq;
using System.Web.Mvc;
using BootstrapMvcSample.Controllers;
using MiniDropbox.Domain;
using MiniDropbox.Domain.Services;
using MiniDropbox.Web.Models;


namespace MiniDropbox.Web.Controllers
{
    public class AccountController : BootstrapBaseController
    {
        private readonly IReadOnlyRepository _readOnlyRepository;
        private readonly IWriteOnlyRepository _writeOnlyRepository;

        public AccountController(IReadOnlyRepository readOnlyRepository, IWriteOnlyRepository writeOnlyRepository)
        {
            _readOnlyRepository = readOnlyRepository;
            _writeOnlyRepository = writeOnlyRepository;
        }

        [HttpGet]
        public ActionResult LogIn()
        {
            return View(new AccountLoginModel());
        }

        [HttpPost]
        public ActionResult LogIn(AccountLoginModel model)
        {
            if (model.EMail == "Admin" && model.Password == "Admin4321")
            {
                Session["userType"] = "Admin";
                return RedirectToAction("RegisteredUsersList", "RegisteredUsersList");
            }

            var passwordEncripted = EncriptacionMD5.Encriptar(model.Password);
            var result = _readOnlyRepository.Query<Account>(x => x.EMail == model.EMail && x.Password==passwordEncripted);

            if (result.Any())
            {
                Session["userId"] = result.FirstOrDefault().Id;
                Session["userType"] = "User";
                return RedirectToAction("ListAllContent", "Disk");
            }
                
           
            Error("E-Mail or Password is incorrect!!!");
            return View();
        }


        public ActionResult SignUp()
        {
            return RedirectToAction("AccountSignUp", "AccountSignUp");
        }

        public ActionResult PasswordRecovery()
        {
            return RedirectToAction("PasswordRecovery", "PasswordRecovery");
        }

        public ActionResult LogOut()
        {
            Session.Clear();
            return RedirectToAction("LogIn", "Account");
        }
        
    }
}