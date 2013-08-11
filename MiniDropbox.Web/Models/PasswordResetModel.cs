using System.ComponentModel.DataAnnotations;

namespace MiniDropbox.Web.Models
{
    public class PasswordResetModel
    {
        [Required]
        public string Password { get; set; }

        [Required]
        public string PasswordConfirm { get; set; }
    }    
}