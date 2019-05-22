namespace OfficeAppointmentSoftware.Entities.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Appointment")]
    public partial class Appointment
    {
        public int AppointmentID { get; set; }
        public short? ExpertID { get; set; }

        public int? ClientID { get; set; }

        [Required]
        [StringLength(100)]
        public string Subject { get; set; }

        [StringLength(450)]
        public string Description { get; set; }

        public DateTime StartTime { get; set; }

        public DateTime? EndTime { get; set; }

        [StringLength(50)]
        public string ThemeColor { get; set; }

        public bool? IsFullDay { get; set; }

        public virtual Client Client { get; set; }

        public virtual Expert Expert { get; set; }
    }
}
