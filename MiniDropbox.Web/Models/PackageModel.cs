using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MiniDropbox.Web.Models
{
    public class PackageModel
    {
        public long Id { get; set; }

        [StringLength(15, ErrorMessage = "6 characters minimum and 15 maximum",MinimumLength = 6)]
        public string Name { get; set; }

        [StringLength(50, ErrorMessage = "25 characters minimum and 50 maximum", MinimumLength = 25)]
        public string Description { get; set; }

        [DataType(DataType.Currency, ErrorMessage = "Invalid format!")]
        public double Price { get; set; } 

        [RegularExpression("[0-9]+",ErrorMessage = "Invalid format!")]
        public int SpaceLimit { get; set; }

        [RegularExpression("[0-9]+", ErrorMessage = "Invalid format!")]
        public int DaysAvailable { get; set; }

        public bool IsArchived { get; set; }
    }
}
