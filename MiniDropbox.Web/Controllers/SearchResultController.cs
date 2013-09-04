using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AutoMapper;
using BootstrapMvcSample.Controllers;
using MiniDropbox.Domain;
using MiniDropbox.Domain.Services;
using MiniDropbox.Web.Models;

namespace MiniDropbox.Web.Controllers
{
    public class SearchResultController : BootstrapBaseController
    {
        private readonly IReadOnlyRepository _readOnlyRepository;
        private readonly IWriteOnlyRepository _writeOnlyRepository;

        public SearchResultController(IWriteOnlyRepository writeOnlyRepository, IReadOnlyRepository readOnlyRepository)
        {
            _readOnlyRepository = readOnlyRepository;
            _writeOnlyRepository = writeOnlyRepository;
        }
    
        public ActionResult ListFileSearchResult(string searchTxt)
        {
            var userData = _readOnlyRepository.First<Account>(x=>x.EMail==User.Identity.Name);
            var filesList = new List<FileSearchResult>();

            foreach (var file in userData.Files)
            {
                if (file.Name.ToLower().Contains(searchTxt.ToLower()))
                {
                    filesList.Add(Mapper.Map<File,FileSearchResult>(file));
                }
            }

            if (!filesList.Any())
            { 
                Error("No results found!!!");
                filesList.Add(new FileSearchResult{Name = " ",Type = " ", CreatedDate = DateTime.Now, Url = string.Empty});
            } 


            return View(filesList);
        }

    }
}
