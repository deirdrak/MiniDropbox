
using System.ComponentModel.DataAnnotations;
using System.Net.Mail;
using Microsoft.Web.Mvc;

namespace MiniDropbox.Web.Models
{
    public class PasswordRecoveryModel
    {
        [Required(ErrorMessage = "Required field", AllowEmptyStrings = false)]
        [Display(Name = "E-Mail Address: ")]
        [EmailAddress(ErrorMessage = "Invalid format")]
        public string EMailAddress { get; set; }
    }
}