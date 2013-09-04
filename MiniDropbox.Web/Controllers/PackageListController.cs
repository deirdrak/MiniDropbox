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
    public class PackageListController : BootstrapBaseController
    {
        private readonly IReadOnlyRepository _readOnlyRepository;
        private readonly IWriteOnlyRepository _writeOnlyRepository;

        public PackageListController(IReadOnlyRepository readOnlyRepository, IWriteOnlyRepository writeOnlyRepository)
        {
            _readOnlyRepository = readOnlyRepository;
            _writeOnlyRepository = writeOnlyRepository;
        }
        //
        // GET: /PackageList/
        [HttpGet]
        public ActionResult PackageList()
        {
            //if (Session["userType"].ToString() != "Admin")
            //{
            //    return null;
            //}

            var packages = _readOnlyRepository.GetAll<Package>();
            var packagesModelList = new List<PackageModel>();

            if (!packages.Any())
            {
                packagesModelList.Add(new PackageModel { Name = "Create packages" });
                return View(packagesModelList);
            }

            foreach (var package in packages)
            {
                packagesModelList.Add(new PackageModel
                {
                    Id = package.Id,
                    IsArchived = package.IsArchived,
                    Name = package.Name,
                    Description = package.Description,
                    Price=package.Price,
                    DaysAvailable = package.DaysAvailable,
                    SpaceLimit = package.SpaceLimit
                });
            }

            return View(packagesModelList);
        }

            public ActionResult CreatePackage()
            {
                return RedirectToAction("CreateEditPackage", "CreateEditPackage", new { id = 0 });
            }

            public ActionResult EditPackage(long packageId)
            {
                return RedirectToAction("CreateEditPackage", "CreateEditPackage", new { id = packageId });
            }

            public ActionResult DeactivatePackage(long packageId)
            {
                var packageData = _readOnlyRepository.GetById<Package>(packageId);

                packageData.IsArchived = !packageData.IsArchived;

                _writeOnlyRepository.Update(packageData);

                return RedirectToAction("PackageList");
            }

    }
}
