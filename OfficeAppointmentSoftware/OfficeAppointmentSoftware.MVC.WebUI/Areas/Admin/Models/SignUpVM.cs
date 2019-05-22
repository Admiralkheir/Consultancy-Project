using OfficeAppointmentSoftware.Entities.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OfficeAppointmentSoftware.MVC.WebUI.Areas.Admin.Models
{
    public class SignUpVM
    {
        //[Required(ErrorMessage = "Bu alanı doldurmak zorunludur")]
        //[MaxLength(20, ErrorMessage = "Bu alan maksimum 20 karakter uzunluğunda olabilir")]
        //[Display(Name = "Ad")]
        //public string Name { get; set; }

        //[Required(ErrorMessage = "Bu alanı doldurmak zorunludur")]
        //[MaxLength(20, ErrorMessage = "Bu alan maksimum 20 karakter uzunluğunda olabilir")]
        //[Display(Name = "Soyad")]
        //public string Surname { get; set; }

        //[Required(ErrorMessage = "Bu alanı doldurmak zorunludur")]
        //[MaxLength(10, ErrorMessage = "Bu alan maksimum 10 karakter uzunluğunda olabilir")]
        //[Display(Name = "Kullanıcı Adı")]
        public string Username { get; set; }

        //[Required(ErrorMessage = "Bu alanı doldurmak zorunludur")]
        //[MaxLength(40, ErrorMessage = "Bu alan maksimum 40 karakter uzunluğunda olabilir")]
        //[EmailAddress(ErrorMessage = "Yanlış E-Posta Formatı. Örn: fixit@fixit.com")]
        public string Email { get; set; }

        //[Required(ErrorMessage = "Bu alanı doldurmak zorunludur")]
        //[MinLength(6, ErrorMessage = "Bu alan minimum 6 karakter uzunluğunda olabilir")]
        //[Display(Name = "Şifre")]
        public string Password { get; set; }

        //[Required(ErrorMessage = "Bu alanı doldurmak zorunludur")]
        //[MinLength(6, ErrorMessage = "Bu alan minimum 6 karakter uzunluğunda olabilir")]
        //[Display(Name = "Şifre Tekrar")]
        //[Compare("Password", ErrorMessage = "Şifreler uyuşmuyor")]
        public string ConfirmPassword { get; set; }
        public Company NewCompany { get; set; }
        public Contact NewContact { get; set; }
        public IEnumerable<SelectListItem> DropDownList { get; set; }
        public int SelectedPlanID { get; set; }


    }
}