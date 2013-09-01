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
using File = MiniDropbox.Domain.File;

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
            var userFiles = _readOnlyRepository.First<Account>(x=>x.EMail==User.Identity.Name).Files;

            var userContent = new List<DiskContentModel>();

            foreach (var file in userFiles)
            {
                if (file == null)
                    continue;

                if(!file.IsArchived)
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

        [HttpGet]
        public PartialViewResult FileUploadModal()
        {
            return PartialView();
        }

        [HttpPost]
        public ActionResult FileUpload(HttpPostedFileBase fileControl)
        {
            if (fileControl == null)
            {
                Error("There was a problem uploading the file :( , please try again!!!");
                return RedirectToAction("ListAllContent");
            }

            var fileSize = fileControl.ContentLength;

            if (fileSize > 10485760)
            {
                Error("The file must be of 10 MB or less!!!");
                return RedirectToAction("ListAllContent");
            }

            var userData = _readOnlyRepository.First<Account>(x => x.EMail == User.Identity.Name);

            var fileName = Path.GetFileName(fileControl.FileName);
            var serverFolderPath = Server.MapPath("~/App_Data/UploadedFiles/"+userData.EMail+"/");
            var directoryInfo = new DirectoryInfo(serverFolderPath);

            if (!directoryInfo.Exists)
            {
                directoryInfo.Create();
            }

            var path = Path.Combine(serverFolderPath, fileName);

            var fileInfo = new DirectoryInfo(serverFolderPath);

            if (fileInfo.Exists)
            {
                var bddInfo = userData.Files.FirstOrDefault(f => f.Name == fileName);
                bddInfo.ModifiedDate = DateTime.Now;
                bddInfo.Type = fileControl.ContentType;
                bddInfo.FileSize = fileSize;
            }
            else
            {
                userData.Files.Add(new File
                {
                    Name = fileName,
                    CreatedDate = DateTime.Now,
                    ModifiedDate = DateTime.Now,
                    FileSize = fileSize,
                    Type = fileControl.ContentType,
                    Url = serverFolderPath,
                    IsArchived = false
                });
            }

            fileControl.SaveAs(path);
            _writeOnlyRepository.Update(userData);

            Success("File uploaded successfully!!! :D");
            return RedirectToAction("ListAllContent");
        }

        public ActionResult DeleteFile(int fileId)
        {
            var userData =_readOnlyRepository.First<Account>(a => a.EMail == User.Identity.Name);
            var fileToDelete = userData.Files.FirstOrDefault(f => f.Id == fileId);

            if (fileToDelete != null)
            {
                
                System.IO.File.Delete(fileToDelete.Url+fileToDelete.Name);

                fileToDelete.IsArchived = true;

                _writeOnlyRepository.Update(userData);
            }

            return RedirectToAction("ListAllContent");
        }
    }
}