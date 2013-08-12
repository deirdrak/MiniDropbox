using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace MiniDropbox.Web.Models
{
    public class PasswordResetModel
    {
        public long UserId { get; set; }

        [Display(Name = "Password: ")]
        [Required(ErrorMessage = "Required field", AllowEmptyStrings = false)]
        [DataType(DataType.Password)]
        [StringLength(25, MinimumLength = 6, ErrorMessage = "The password must be 6 characters minimum")]
        [RegularExpression("^[a-zA-Z0-9]+$", ErrorMessage = "La contraseña debe contener numeros y letras(no caracteres especiales)")]
        public string Password { get; set; }

        [Display(Name = "Confirm Password:")]
        [Required(ErrorMessage = "Required field", AllowEmptyStrings = false)]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "The passwords don't match!")]
        public string PasswordConfirm { get; set; }
    }    
}