namespace OfficeAppointmentSoftware.Entities.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Company")]
    public partial class Company
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Company()
        {
            Client = new HashSet<Client>();
            Contact = new HashSet<Contact>();
            Expert = new HashSet<Expert>();
        }

        public int CompanyID { get; set; }

        public short PlanID { get; set; }

        [StringLength(300)]
        public string LogoURL { get; set; }

        [Required]
        [StringLength(200)]
        public string CompanyName { get; set; }

        [Required]
        [StringLength(150)]
        public string OwnerName { get; set; }

        [StringLength(450)]
        public string Description { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Client> Client { get; set; }

        public virtual Plans Plans { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Contact> Contact { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Expert> Expert { get; set; }
    }
}
