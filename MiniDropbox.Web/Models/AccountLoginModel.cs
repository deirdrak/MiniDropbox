using System.ComponentModel.DataAnnotations;

namespace MiniDropbox.Web.Models
{
    public class AccountLoginModel
    {
        [Display(Name = "E-Mail: ")]
        [Required(ErrorMessage = "Required Field")]
        public string EMail { get; set; }

        [Display(Name = "Password: ")]
        [Required(ErrorMessage = "Required Field")]
        public string Password { get; set; }

        public bool RememberMe { get; set; }
    }
}