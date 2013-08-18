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
    public class CreateEditPackageController : BootstrapBaseController
    {
        private readonly IReadOnlyRepository _readOnlyRepository;
        private readonly IWriteOnlyRepository _writeOnlyRepository;

        public CreateEditPackageController(IReadOnlyRepository readOnlyRepository, IWriteOnlyRepository writeOnlyRepository)
        {
            _readOnlyRepository = readOnlyRepository;
            _writeOnlyRepository = writeOnlyRepository;
        }

        //
        // GET: /CreateEditPackage/
   
        [HttpGet]
        public ActionResult CreateEditPackage(long id)
        {
            if (Session["userType"].ToString() != "Admin")
            {
                return null;
            }

            if (id == 0)
            {
                Session["createPackageIsActive"] = true;
                return View(new PackageModel());    
            }

            Session["createPackageIsActive"] = false;
            var packageData = _readOnlyRepository.GetById<Package>(id);

            return View(new PackageModel
            {
                Id=packageData.Id,
                IsArchived = packageData.IsArchived,
                Name = packageData.Name,
                Description = packageData.Description,
                SpaceLimit = packageData.SpaceLimit,
                Price = packageData.Price,
                DaysAvailable = packageData.DaysAvailable
            });
        }

        public ActionResult Cancel()
        {
            return RedirectToAction("PackageList", "PackageList");
        }

        [HttpPost]
        public ActionResult CreateEditPackage(PackageModel model)
        {
            if (Convert.ToBoolean(Session["createPackageIsActive"]))
            {
                var newPackage = new Package
                {
                    Id = model.Id,
                    IsArchived = model.IsArchived,
                    Name = model.Name,
                    Description = model.Description,
                    Price=model.Price,
                    SpaceLimit = model.SpaceLimit,
                    CreationDate = DateTime.Now.Date,
                    DaysAvailable = model.DaysAvailable
                };

                _writeOnlyRepository.Create(newPackage);
            }
            else
            {
                var packageData = _readOnlyRepository.GetById<Package>(model.Id);
                packageData.Name = model.Name;
                packageData.SpaceLimit = model.SpaceLimit;
                packageData.IsArchived = model.IsArchived;
                packageData.Price = model.Price;
                packageData.Description = model.Description;
                packageData.DaysAvailable = model.DaysAvailable;

                _writeOnlyRepository.Update(packageData);
            }

            return RedirectToAction("PackageList", "PackageList");
            
        }

    }
}
