using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using OfficeAppointmentSoftware.Entities.Entities;

namespace OfficeAppointmentSoftware.MVC.WebUI.Models
{
    public class UserContact
    {
        [Required(ErrorMessage = "Lütfen İsim ve Soyad Giriniz.")]
        [MaxLength(40, ErrorMessage = "Ad ve Soyad maksimum 30 Haneden Oluşur.")]
        public string NameSurname { get; set; }
        [Required(ErrorMessage = "Lütfen Konu Giriniz.")]
        [MaxLength(30, ErrorMessage = "Konu maksimum 30 Haneden Oluşur.")]
        public string Subject { get; set; }
        [Required(ErrorMessage = "Lütfen EMail Giriniz.")]
        //[EmailAddress(ErrorMessage ="Düzgün Email Giriniz")]Kendisinin belirlediğin email formatına göre bakar.
        [EmailAddress(ErrorMessage = "Doğru Formatta Giriniz.")]//custom regular expression email formatı için
        public string EMail { get; set; }

        [Required(ErrorMessage = "Lütfen Mesaj Giriniz.")]
        [MaxLength(1000, ErrorMessage = "Mesaj maksimum 30 Haneden Oluşur.")]
        public string Message { get; set; }

        public Banner Banner { get; set; }
        public IEnumerable<Works> Works { get; set; }
        public IEnumerable<Features> Features { get; set; }
    }
}