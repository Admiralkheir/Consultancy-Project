namespace OfficeAppointmentSoftware.Entities.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Plans
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Plans()
        {
            Company = new HashSet<Company>();
        }

        [Key]
        public short PlanID { get; set; }

        [Required]
        [StringLength(50)]
        public string PlanName { get; set; }

        public decimal Price { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Company> Company { get; set; }
    }
}
