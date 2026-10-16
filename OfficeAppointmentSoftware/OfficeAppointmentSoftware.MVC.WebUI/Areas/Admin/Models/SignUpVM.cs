using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.Rendering;
using OfficeAppointmentSoftware.Entities.Entities;
using System.ComponentModel.DataAnnotations;

namespace OfficeAppointmentSoftware.MVC.WebUI.Areas.Admin.Models
{
    // Creates an office: its company record, contact details and login user.
    public class SignUpVM
    {
        [Required]
        [StringLength(50)]
        [RegularExpression(@"^[a-zA-Z0-9._@+-]+$", ErrorMessage = "Kullanıcı adı yalnızca harf, rakam ve . _ @ + - içerebilir.")]
        [Display(Name = "Kullanıcı adı")]
        public string Username { get; set; }

        [Required]
        [StringLength(100, MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Şifre")]
        public string Password { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Compare(nameof(Password), ErrorMessage = "Şifreler eşleşmiyor.")]
        [Display(Name = "Şifre (tekrar)")]
        public string ConfirmPassword { get; set; }

        public Company NewCompany { get; set; } = new Company();
        public Contact NewContact { get; set; } = new Contact();

        [ValidateNever]
        public IEnumerable<SelectListItem> DropDownList { get; set; } = Enumerable.Empty<SelectListItem>();

        [Range(1, short.MaxValue, ErrorMessage = "Bir paket seçiniz.")]
        [Display(Name = "Paket")]
        public int SelectedPlanID { get; set; }
    }
}
