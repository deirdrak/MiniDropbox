using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using BootstrapMvcSample.Controllers;
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
            long userId = Convert.ToInt64(Session["userId"]);
            var userFiles = _readOnlyRepository.GetById<Account>(userId).Files;

            var userContent = new List<DiskContentModel>();

            foreach (var file in userFiles)
            {
                if (file == null)
                    continue;

                userContent.Add(new DiskContentModel
                {
                    Id = file.Id,
                    ModifiedDate = file.ModifiedDate,
                    Name = file.Name,
                    Type = file.Type
                });
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
            
            //var listOfContent = Builder<DiskContentModel>.CreateListOfSize(10).Build().ToList(); 
            return View(userContent);
        }
    }
}