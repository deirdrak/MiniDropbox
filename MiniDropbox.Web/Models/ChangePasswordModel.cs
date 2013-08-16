using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MiniDropbox.Web.Models
{
    public class ChangePasswordModel
    {
        [Display(Name = "Old Password: ")]
        [Required(ErrorMessage = "Required field", AllowEmptyStrings = false)]
        [DataType(DataType.Password)]
        public string OldPassword { get; set; }

        [Display(Name = "New Password: ")]
        [Required(ErrorMessage = "Required field", AllowEmptyStrings = false)]
        [DataType(DataType.Password)]
        [StringLength(25, MinimumLength = 6, ErrorMessage = "The password must be 6 characters minimum")]
        [RegularExpression("^[a-zA-Z0-9]+$", ErrorMessage = "La contraseña debe contener numeros y letras(no caracteres especiales)")]
        public string NewPassword { get; set; }

        [Display(Name = "Confirm Password: ")]
        [Required(ErrorMessage = "Required field", AllowEmptyStrings = false)]
        [DataType(DataType.Password)]
        [Compare("NewPassword", ErrorMessage = "The passwords don't match!")]
        public string ConfirmPassword { get; set; }
    }
}