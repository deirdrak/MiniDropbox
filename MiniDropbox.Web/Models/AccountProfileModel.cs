using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MiniDropbox.Web.Models
{
    public class AccountProfileModel
    {
        public string Name { get; set; }
        public string LastName { get; set; }
        public string Password { get; set; }
        public string EMail { get; set; }
        public DateTime BirthDate { get; set; }
        public Byte[] Picture { get; set; }
    }
}