using System;

namespace MiniDropbox.Web.Models
{
    public class DiskContentModel
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }
        public DateTime ModifiedDate { get; set; }
    }
}