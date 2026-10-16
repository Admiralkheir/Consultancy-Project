using System.ComponentModel.DataAnnotations;

namespace OfficeAppointmentSoftware.MVC.WebUI.Models
{
    public class LoginVM
    {
        [Required]
        [Display(Name = "Kullanıcı adı")]
        public string UserName { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Şifre")]
        public string Password { get; set; }

        [Display(Name = "Beni hatırla")]
        public bool RememberMe { get; set; }
    }
}
