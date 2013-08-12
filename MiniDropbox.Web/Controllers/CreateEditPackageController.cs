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
                SpaceLimit = packageData.SpaceLimit
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
                    SpaceLimit = model.SpaceLimit
                };

                _writeOnlyRepository.Create(newPackage);
            }
            else
            {
                var packageData = _readOnlyRepository.GetById<Package>(model.Id);
                packageData.Name = model.Name;
                packageData.SpaceLimit = model.SpaceLimit;
                packageData.IsArchived = model.IsArchived;

                _writeOnlyRepository.Update(packageData);
            }

            return RedirectToAction("PackageList", "PackageList");
            
        }

    }
}
