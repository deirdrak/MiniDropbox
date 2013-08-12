using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MiniDropbox.Web.Models
{
    public class PackageModel
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public int SpaceLimit { get; set; }
        public bool IsArchived { get; set; }
    }
}