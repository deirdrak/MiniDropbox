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
            //var actualPath = Session["ActualPath"].ToString();
            var actualFolder = Session["ActualFolder"].ToString();
            var userFiles = _readOnlyRepository.First<Account>(x=>x.EMail==User.Identity.Name).Files;

            var userContent = new List<DiskContentModel>();

            foreach (var file in userFiles)
            {
                if (file == null)
                    continue;

                var fileFolder = file.Url.Split('\\').LastOrDefault();

                if(!file.IsArchived && fileFolder.Equals(actualFolder) && !string.Equals(file.Name,actualFolder))
                    userContent.Add(Mapper.Map<DiskContentModel>(file));
            }

            if (userContent.Count == 0)
            {
                userContent.Add(new DiskContentModel
                {
                    Id =0,
                    ModifiedDate = DateTime.Now.Date,
                    Name = "You can now add files to your account",
                    Type = "none"
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
            var actualPath = Session["ActualPath"].ToString();
            var fileName = Path.GetFileName(fileControl.FileName);

            var serverFolderPath = Server.MapPath("~/App_Data/UploadedFiles/"+actualPath);
            //var directoryInfo = new DirectoryInfo(serverFolderPath);

            //if (!directoryInfo.Exists)
            //{
            //    directoryInfo.Create();
            //}

            //var sharedDirectory = new DirectoryInfo( Server.MapPath("~/App_Data/UploadedFiles/"+User.Identity.Name + "/Shared"));
            //if (!sharedDirectory.Exists)
            //{
            //    sharedDirectory.Create();
            //    userData.Files.Add(new File
            //    {
            //        Name = "Shared",
            //        CreatedDate = DateTime.Now,
            //        ModifiedDate = DateTime.Now,
            //        FileSize = 0,
            //        Type = "",
            //        Url = sharedDirectory.FullName,
            //        IsArchived = false,
            //        IsDirectory = true
            //    });
            //}

            var path = Path.Combine(serverFolderPath, fileName);

            var fileInfo = new DirectoryInfo(serverFolderPath+fileName);

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
                    IsArchived = false,
                    IsDirectory = false
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

        [HttpPost]
        public ActionResult CreateFolder(string folderName)
        {
            if (folderName.Length > 25)
            {
                Error("Folder name should be 25 characters maximum!!!");
                return RedirectToAction("ListAllContent");
            }

            var userData = _readOnlyRepository.First<Account>(x => x.EMail == User.Identity.Name);

            if (folderName == userData.EMail)
            {
                Error("Folder already exists!!!");
                return RedirectToAction("ListAllContent");
            }

            var actualPath = Session["ActualPath"].ToString();
            var serverFolderPath = Server.MapPath("~/App_Data/UploadedFiles/" + actualPath + "/"+folderName);

            var folderInfo = new DirectoryInfo(serverFolderPath);

            if (folderInfo.Exists)
            {
                Error("Folder already exists!!!");
                return RedirectToAction("ListAllContent");
            }

            
            userData.Files.Add(new File
            {
                Name = folderName,
                CreatedDate = DateTime.Now,
                FileSize = 0,
                IsArchived = false,
                IsDirectory = true,
                ModifiedDate = DateTime.Now,
                Type = "",
                Url = Server.MapPath("~/App_Data/UploadedFiles/" + actualPath)
            });

            var result=Directory.CreateDirectory(serverFolderPath);

            if(!result.Exists)
                Error("The folder was not created!!! Try again please!!!");
            else
            {
                Success("The folder was created successfully!!!");
                _writeOnlyRepository.Update(userData);
            }

            return RedirectToAction("ListAllContent");
        }
        
        public ActionResult ListFolderContent(string folderName)
        {
            Session["ActualPath"] += "/" + folderName;
            Session["ActualFolder"] = folderName;
            return RedirectToAction("ListAllContent");
        }

        public ActionResult ListAllContentRoot()
        {
            Session["ActualPath"] = User.Identity.Name;
            Session["ActualFolder"] = User.Identity.Name;
            return RedirectToAction("ListAllContent");
        }

        public ActionResult DownloadFile(int fileId)
        {
            var fileData = _readOnlyRepository.First<Account>(x => x.EMail == User.Identity.Name).Files.FirstOrDefault(f => f.Id == fileId);
            
            var template_file = System.IO.File.ReadAllBytes(fileData.Url+"/"+fileData.Name);
        
            return new FileContentResult(template_file, fileData.Type){
            FileDownloadName = fileData.Name};

        }
        
    }
}