using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Amazon.S3.Model;
using AutoMapper;
using BootstrapMvcSample.Controllers;
using MiniDropbox.Domain;
using MiniDropbox.Domain.Services;
using MiniDropbox.Web.Models;
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
            var userData = _readOnlyRepository.First<Account>(x => x.EMail == User.Identity.Name);
            var userFiles = userData.Files;

            var userContent = new List<DiskContentModel>();

            foreach (var file in userFiles)
            {
                if (file == null)
                    continue;

                var fileFolderArray = file.Url.Split('/');
                var fileFolder =fileFolderArray.Length>1?fileFolderArray[fileFolderArray.Length-2]:fileFolderArray.FirstOrDefault();

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

            //var serverFolderPath = Server.MapPath("~/App_Data/UploadedFiles/"+actualPath);
           
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

            //var path = Path.Combine(serverFolderPath, fileName);

            //var fileInfo = new DirectoryInfo(serverFolderPath+fileName);

            if (userData.Files.Count(l=>l.Name==fileName && l.Url.EndsWith(actualPath) && !l.IsArchived)>0)//Actualizar Info Archivo
            {
                var bddInfo = userData.Files.FirstOrDefault(f => f.Name == fileName);
                bddInfo.ModifiedDate = DateTime.Now;
                bddInfo.Type = fileControl.ContentType;
                bddInfo.FileSize = fileSize;
                _writeOnlyRepository.Update(bddInfo);
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
                    Url = actualPath,
                    IsArchived = false,
                    IsDirectory = false
                });
                _writeOnlyRepository.Update(userData);
            }

            //fileControl.SaveAs(path);
            var putObjectRequest = new PutObjectRequest
            {
                BucketName = userData.BucketName,
                Key = actualPath + fileName,
                InputStream = fileControl.InputStream
            };

            AWSClient.PutObject(putObjectRequest);

            Success("File uploaded successfully!!! :D");
            return RedirectToAction("ListAllContent");
        }

        public ActionResult DeleteFile(int fileId)
        {
            var userData =_readOnlyRepository.First<Account>(a => a.EMail == User.Identity.Name);
            var fileToDelete = userData.Files.FirstOrDefault(f => f.Id == fileId);

            if (fileToDelete != null)
            {
                if (!fileToDelete.IsDirectory)
                {
                    var deleteRequest = new DeleteObjectRequest
                    {
                        BucketName = userData.BucketName,
                        Key = fileToDelete.Url + fileToDelete.Name
                    };
                    AWSClient.DeleteObject(deleteRequest);
                    fileToDelete.IsArchived = true;
                }
                else//Borrar carpeta con todos sus archivos
                {
                    DeleteFolder(fileToDelete.Id);
                    //var filesList = new List<KeyVersion>();
                    //var userFiles = userData.Files;

                    //foreach (var file in userFiles)
                    //{
                    //    if (file == null)
                    //        continue;

                    //    var fileFolderArray = file.Url.Split('/');
                    //    var fileFolder = fileFolderArray.Length > 1 ? fileFolderArray[fileFolderArray.Length - 2] : fileFolderArray.FirstOrDefault();

                    //    if (!file.IsArchived && fileFolder.Equals(fileToDelete.Name) &&
                    //        !string.Equals(file.Name, fileToDelete.Name))
                    //    {
                    //        filesList.Add(!file.IsDirectory ? new KeyVersion(file.Url + file.Name) : new KeyVersion(file.Url+file.Name + "/"));
                    //        file.IsArchived = true;
                    //    }

                    fileToDelete.IsArchived = true;
                    //}

                    //filesList.Add(new KeyVersion(fileToDelete.Name+"/"));

                    //var deleteRequest = new DeleteObjectsRequest
                    //{
                    //    BucketName = userData.BucketName,
                    //    Keys = filesList
                    //};

                    //AWSClient.DeleteObjects(deleteRequest);

                        var deleteRequest = new DeleteObjectRequest
                        {
                            BucketName = userData.BucketName,
                            Key = fileToDelete.Url + fileToDelete.Name+"/"
                        };
                        AWSClient.DeleteObject(deleteRequest);
                }
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
            
            if (userData.Files.Count(l=>l.Name==folderName)>0)
            {
                Error("Folder already exists!!!");
                return RedirectToAction("ListAllContent");
            }

            var actualPath = Session["ActualPath"].ToString();

            var putFolder = new PutObjectRequest { BucketName = userData.BucketName, Key = actualPath+folderName+"/", ContentBody = string.Empty };
            AWSClient.PutObject(putFolder);
            //var serverFolderPath = Server.MapPath("~/App_Data/UploadedFiles/" + actualPath + "/"+folderName);

            //var folderInfo = new DirectoryInfo(serverFolderPath);

            //if (folderInfo.Exists)
            //{
            //    Error("Folder already exists!!!");
            //    return RedirectToAction("ListAllContent");
            //}

            
            userData.Files.Add(new File
            {
                Name = folderName,
                CreatedDate = DateTime.Now,
                FileSize = 0,
                IsArchived = false,
                IsDirectory = true,
                ModifiedDate = DateTime.Now,
                Type = "",
                Url = actualPath
            });

            //var result=Directory.CreateDirectory(serverFolderPath);

            //if(!result.Exists)
            //    Error("The folder was not created!!! Try again please!!!");
            //else
            //{
                Success("The folder was created successfully!!!");
                _writeOnlyRepository.Update(userData);
            //}

            return RedirectToAction("ListAllContent");
        }

        public void DeleteFolder(long folderId)
        {
            var userData = _readOnlyRepository.First<Account>(a => a.EMail == User.Identity.Name);
            var folderToDelete = userData.Files.FirstOrDefault(f => f.Id == folderId);

            //if (!folderToDelete.IsDirectory)
            //    {
            //        var deleteRequest = new DeleteObjectRequest
            //        {
            //            BucketName = userData.BucketName,
            //            Key = fileToDelete.Url + fileToDelete.Name
            //        };
            //        AWSClient.DeleteObject(deleteRequest);
            //        fileToDelete.IsArchived = true;
            //    }
            //    else
            //    {
                    var userFiles = userData.Files.Where(t=>t.Url.Contains(folderToDelete.Name));

                    foreach (var file in userFiles)
                    {
                        if (file == null)
                            continue;

                        if(file.IsDirectory)
                            DeleteFolder(file.Id);
                        
                        var fileFolderArray = file.Url.Split('/');
                        var fileFolder = fileFolderArray.Length > 1 ? fileFolderArray[fileFolderArray.Length - 2] : fileFolderArray.FirstOrDefault();

                        if (!file.IsArchived && fileFolder.Equals(folderToDelete.Name) &&
                            !string.Equals(file.Name, folderToDelete.Name))
                        {
                            var deleteRequest = new DeleteObjectRequest
                            {
                                BucketName = userData.BucketName,
                                Key = file.Url + file.Name
                            };
                            AWSClient.DeleteObject(deleteRequest);
                            file.IsArchived = true;
                            _writeOnlyRepository.Update(userData);
                        }
                    }

                    folderToDelete.IsArchived = true;
                    var deleteRequest2 = new DeleteObjectRequest
                    {
                        BucketName = userData.BucketName,
                        Key = folderToDelete.Url + folderToDelete.Name + "/"
                    };
                    AWSClient.DeleteObject(deleteRequest2);
                    _writeOnlyRepository.Update(userData);
            //}
            

        }
        
        public ActionResult ListFolderContent(string folderName)
        {
            Session["ActualPath"] += folderName + "/";
            Session["ActualFolder"] = folderName;
            return RedirectToAction("ListAllContent");
        }

        public ActionResult ListAllContentRoot()
        {
            Session["ActualPath"] = string.Empty;
            Session["ActualFolder"] = string.Empty;
            return RedirectToAction("ListAllContent");
        }

        public ActionResult DownloadFile(int fileId)
        {
            var userData =_readOnlyRepository.First<Account>(a => a.EMail == User.Identity.Name);
            var fileData = userData.Files.FirstOrDefault(f => f.Id == fileId);

            var objectRequest = new GetObjectRequest{BucketName =userData.BucketName,Key = fileData.Url+fileData.Name};
            var file=AWSClient.GetObject(objectRequest);
            var byteArray = new byte[file.ContentLength];
            file.ResponseStream.Read(byteArray, 0,(int)file.ContentLength);
            //var template_file = System.IO.File.ReadAllBytes();

            return new FileContentResult(byteArray, fileData.Type)
            {
            FileDownloadName = fileData.Name};

        }
        
    }
}