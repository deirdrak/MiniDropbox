using System.ComponentModel.DataAnnotations;

namespace MiniDropbox.Web.Models
{
    public class AccountLoginModel
    {
        [Display(Name = "E-Mail: ")]
        [Required]
        public string EMail { get; set; }

        [Display(Name = "Password: ")]
        [Required]
        public string Password { get; set; }
    }
}