using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Services.Description;
using Amazon.S3.Model;
using AutoMapper;
using BootstrapMvcSample.Controllers;
using MiniDropbox.Domain;
using MiniDropbox.Domain.Services;
using MiniDropbox.Web.Models;
using File = System.IO.File;


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
        public ActionResult AccountSignUp(long token)
        {
            Session["userReferralId"] = token;
            
            return View(new AccountSignUpModel());
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

            var account = Mapper.Map<Account>(model);
            account.IsArchived = false;
            account.IsAdmin = false;
            account.IsBlocked = false;
            account.SpaceLimit = 500;
            account.Password = EncriptacionMD5.Encriptar(model.Password);
            account.BucketName = string.Format("mdp.{0}", Guid.NewGuid());

           var createdAccount= _writeOnlyRepository.Create(account);

            var token = Convert.ToInt64(Session["userReferralId"]);

            if (token != 0)
            {
                var userReferring = _readOnlyRepository.GetById<Account>(token);
                userReferring.Referrals.Add(createdAccount);
                _writeOnlyRepository.Update(userReferring);
            }

            
            var newBucket = new PutBucketRequest { BucketName = account.BucketName };
            AWSClient.PutBucket(newBucket);
           
            var putFolder = new PutObjectRequest{BucketName = account.BucketName, Key = "Shared/",ContentBody = string.Empty};
            AWSClient.PutObject(putFolder);

            //var serverFolderPath = Server.MapPath("~/App_Data/UploadedFiles/" + account.EMail);
            //Directory.CreateDirectory(serverFolderPath);

            //var sharedDirectory =serverFolderPath + "/Shared";
            //Directory.CreateDirectory(sharedDirectory);

            if (createdAccount.Files == null)
            {
                createdAccount.Files= new List<Domain.File>();
            }

            createdAccount.Files.Add(new Domain.File
            {
                CreatedDate = DateTime.Now,
                FileSize = 0,
                IsArchived = false,
                IsDirectory = true,
                Name = "Shared",
                Url = "",
                Type = "",
                ModifiedDate = DateTime.Now
            });

            _writeOnlyRepository.Update(createdAccount);
            return Cancelar();
        }

    }
}
