namespace OfficeAppointmentSoftware.Entities.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Expert")]
    public partial class Expert
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Expert()
        {
            Appointment = new HashSet<Appointment>();
            Contact = new HashSet<Contact>();
        }

        public short ExpertID { get; set; }

        public int CompanyID { get; set; }

        [StringLength(300)]
        public string PhotoURL { get; set; }

        [Required]
        [StringLength(50)]
        public string Name { get; set; }

        [Required]
        [StringLength(50)]
        public string Surname { get; set; }

        public DateTime BirthDate { get; set; }

        [Required]
        [StringLength(100)]
        public string Profession { get; set; }

        [Required]
        [StringLength(450)]
        public string WagePolicy { get; set; }

        [StringLength(450)]
        public string Note { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Appointment> Appointment { get; set; }

        public virtual Company Company { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Contact> Contact { get; set; }
    }
}
