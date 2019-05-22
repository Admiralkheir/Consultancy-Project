namespace OfficeAppointmentSoftware.Entities.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Contact")]
    public partial class Contact
    {
        public int ContactID { get; set; }

        public int? CompanyID { get; set; }

        public int? ClientID { get; set; }

        public short? ExpertID { get; set; }

        [Required]
        [StringLength(15)]
        public string PhoneNumber1 { get; set; }

        
        [StringLength(15)]
        public string PhoneNumber2 { get; set; }

        [Required]
        [StringLength(50)]
        public string Email { get; set; }

        [Required]
        [StringLength(250)]
        public string Address { get; set; }

        public bool IsActive { get; set; }

        public virtual Client Client { get; set; }

        public virtual Company Company { get; set; }

        public virtual Expert Expert { get; set; }
    }
}
