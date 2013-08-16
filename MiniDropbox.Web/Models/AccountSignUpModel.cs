using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using Microsoft.Web.Mvc;

namespace MiniDropbox.Web.Models
{
    public class AccountSignUpModel
    {

        [Display(Name = "Name: ")]
        [Required(ErrorMessage = "Required field", AllowEmptyStrings = false)]
        public string Name { get; set; }

        [Display(Name = "Last Name: ")]
        [Required(ErrorMessage = "Required field", AllowEmptyStrings = false)]
        public string LastName { get; set; }

        [Display(Name = "E-Mail: ")]
        [Required(ErrorMessage = "Required field", AllowEmptyStrings = false)]
        [EmailAddress(ErrorMessage = "Invalid format")]
        public string EMail { get; set; }

        [Display(Name = "Password: ")]
        [Required(ErrorMessage = "Required field", AllowEmptyStrings = false)]
        [DataType(DataType.Password)]
        [StringLength(25, MinimumLength = 6,ErrorMessage = "The password must be 6 characters minimum")]
        [RegularExpression("^[a-zA-Z0-9]+$", ErrorMessage = "La contraseña debe contener numeros y letras(no caracteres especiales)")]
        public string Password { get; set; }

        [Display(Name = "Confirm Password: ")]
        [Required(ErrorMessage = "Required field", AllowEmptyStrings = false)]
        [DataType(DataType.Password)]
        [Compare("Password",ErrorMessage = "The passwords don't match!")]
        public string PasswordConfirm { get; set; }

        //public int SpaceLimit { get; private set; }
        //public int UsedSpace { get; private set; }
        //public bool IsArchived { get; private set; }
        //public bool IsBlocked { get; private set; }
    }
}