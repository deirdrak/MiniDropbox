using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MiniDropbox.Web.Models
{
    public class FileSearchResult
    {
        public string Name { get; set; }
        public string Url { get; set; }
        public string Type { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}