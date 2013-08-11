using System.Collections;
using System.Linq;
using System.Web.Mvc;
using BootstrapMvcSample.Controllers;
using FizzWare.NBuilder;
using MiniDropbox.Domain;
using MiniDropbox.Domain.Services;
using MiniDropbox.Web.Models;

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
        public ActionResult ListAllContent(IList userFiles)
        {
            var listOfContent = Builder<DiskContentModel>.CreateListOfSize(10).Build().ToList(); 
            return View(listOfContent);
        }
    }
}