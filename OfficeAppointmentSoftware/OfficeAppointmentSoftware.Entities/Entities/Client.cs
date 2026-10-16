namespace OfficeAppointmentSoftware.Entities.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("Client")]
    public partial class Client
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Client()
        {
            Appointment = new HashSet<Appointment>();
            Contact = new HashSet<Contact>();
        }

        public int ClientID { get; set; }

        public int CompanyID { get; set; }

        [Required]
        [StringLength(15)]
        [RegularExpression(@"^\d{11}$", ErrorMessage = "TC Kimlik No 11 haneli bir sayı olmalıdır.")]
        [Display(Name = "TC Kimlik No")]
        public string TcKimlik { get; set; }

        [Required]
        [StringLength(50)]
        [Display(Name = "Ad")]
        public string Name { get; set; }

        [Required]
        [StringLength(50)]
        [Display(Name = "Soyad")]
        public string Surname { get; set; }

        [Display(Name = "Doğum Tarihi")]
        public DateTime BirthDate { get; set; }

        [Required]
        [StringLength(450)]
        [Display(Name = "Problem")]
        public string Problem { get; set; }

        [StringLength(450)]
        [Display(Name = "Not")]
        public string Note { get; set; }

        [Display(Name = "Devam ediyor")]
        public bool Continuity { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Appointment> Appointment { get; set; }

        public virtual Company Company { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Contact> Contact { get; set; }
    }
}
