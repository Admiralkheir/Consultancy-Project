namespace OfficeAppointmentSoftware.Entities.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("Contact")]
    public partial class Contact
    {
        private const string PhonePattern = @"^[0-9+()\s-]{7,15}$";
        private const string PhoneMessage = "Geçerli bir telefon numarası giriniz (örn. 05551234567).";

        public int ContactID { get; set; }

        public int? CompanyID { get; set; }

        public int? ClientID { get; set; }

        public short? ExpertID { get; set; }

        [Required]
        [StringLength(15)]
        [RegularExpression(PhonePattern, ErrorMessage = PhoneMessage)]
        [Display(Name = "Telefon")]
        public string PhoneNumber1 { get; set; }


        [StringLength(15)]
        [RegularExpression(PhonePattern, ErrorMessage = PhoneMessage)]
        [Display(Name = "İkinci Telefon")]
        public string PhoneNumber2 { get; set; }

        [Required]
        [StringLength(50)]
        [EmailAddress]
        [Display(Name = "E-posta")]
        public string Email { get; set; }

        [Required]
        [StringLength(250)]
        [Display(Name = "Adres")]
        public string Address { get; set; }

        public bool IsActive { get; set; }

        public virtual Client Client { get; set; }

        public virtual Company Company { get; set; }

        public virtual Expert Expert { get; set; }
    }
}
