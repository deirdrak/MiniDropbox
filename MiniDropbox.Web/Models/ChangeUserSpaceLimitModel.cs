using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MiniDropbox.Web.Models
{
    public class ChangeUserSpaceLimitModel
    {
        public long UserId { get; set; }
        public string Name { get;  set; }
        public string LastName { get;  set; }
        public string EMail { get;  set; }

        [Required]
        [RegularExpression("[0-9]*", ErrorMessage = "Only numbers")]
        public int SpaceLimit { get; set; }

        public bool Archived { get;  set; }
        public bool Blocked { get;  set; }

    }
}