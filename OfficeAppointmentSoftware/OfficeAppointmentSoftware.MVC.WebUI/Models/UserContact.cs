using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using OfficeAppointmentSoftware.Entities.Entities;
using System.ComponentModel.DataAnnotations;

namespace OfficeAppointmentSoftware.MVC.WebUI.Models
{
    // Landing page content plus the contact form.
    public class UserContact
    {
        [Required(ErrorMessage = "Lütfen adınızı ve soyadınızı giriniz.")]
        [MaxLength(40, ErrorMessage = "Ad ve soyad en fazla 40 karakter olabilir.")]
        [Display(Name = "Ad Soyad")]
        public string NameSurname { get; set; }

        [Required(ErrorMessage = "Lütfen bir konu giriniz.")]
        [MaxLength(60, ErrorMessage = "Konu en fazla 60 karakter olabilir.")]
        [Display(Name = "Konu")]
        public string Subject { get; set; }

        [Required(ErrorMessage = "Lütfen e-posta adresinizi giriniz.")]
        [EmailAddress(ErrorMessage = "Geçerli bir e-posta adresi giriniz.")]
        [Display(Name = "E-posta")]
        public string EMail { get; set; }

        [Required(ErrorMessage = "Lütfen mesajınızı giriniz.")]
        [MaxLength(1000, ErrorMessage = "Mesaj en fazla 1000 karakter olabilir.")]
        [Display(Name = "Mesaj")]
        public string Message { get; set; }

        [ValidateNever]
        public Banner Banner { get; set; }
        [ValidateNever]
        public IEnumerable<Works> Works { get; set; } = Enumerable.Empty<Works>();
        [ValidateNever]
        public IEnumerable<Features> Features { get; set; } = Enumerable.Empty<Features>();
        [ValidateNever]
        public IEnumerable<Plans> Plans { get; set; } = Enumerable.Empty<Plans>();
    }
}
