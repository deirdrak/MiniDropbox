using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using AutoMapper;
using BootstrapMvcSample.Controllers;
using BootstrapSupport;
using FizzWare.NBuilder;
using FluentNHibernate.Testing.Values;
using MiniDropbox.Domain;
using MiniDropbox.Domain.Services;
using MiniDropbox.Web.Models;
using NHibernate.Mapping;

namespace MiniDropbox.Web.Controllers
{
    public class DiskController : BootstrapBaseController
    {
        private readonly IReadOnlyRepository _readOnlyRepository;
        private readonly IWriteOnlyRepository _writeOnlyRepository;

        public DiskController(IWriteOnlyRepository writeOnlyRepository, IReadOnlyRepository readOnlyRepository)
        {
            _readOnlyRepository = readOnlyRepository;
            _writeOnlyRepository = writeOnlyRepository;
        }

        [HttpGet]
        public ActionResult ListAllContent()
        {
            
            //long userId = Convert.ToInt64(Session["userId"]);
            var Xx = User.Identity;
            var userFiles = _readOnlyRepository.First<Account>(x=>x.EMail==User.Identity.Name).Files;

            var userContent = new List<DiskContentModel>();

            foreach (var file in userFiles)
            {
                if (file == null)
                    continue;

                userContent.Add(Mapper.Map<DiskContentModel>(file));
            }

            if (userContent.Count == 0)
            {
                userContent.Add(new DiskContentModel
                {
                    Id =0,
                    ModifiedDate = DateTime.Now.Date,
                    Name = "You can now add files to your account",
                    Type = ""
                });
            }

            return View(userContent);
        }

        [HttpPost]
        public ActionResult FileUpload(HttpPostedFileBase fileControl)
        {
            if (fileControl == null)
            {
                Error("There was a problem uploading the file :( , please try again!!!");
                return RedirectToAction("ListAllContent");
            }

            var x = fileControl.ContentLength;
            var y = fileControl.InputStream.Length;

            var fileName = Path.GetFileName(fileControl.FileName);
            var serverFolderPath = Server.MapPath("~/App_Data/UploadedFiles/");
            var directoryInfo = new DirectoryInfo(serverFolderPath);

            if (!directoryInfo.Exists)
            {
                directoryInfo.Create();
            }

            var path = Path.Combine(serverFolderPath, fileName);

            fileControl.SaveAs(path);

            Success("File uploaded successfully!!! :D");
            return RedirectToAction("ListAllContent");
        }

    }
}