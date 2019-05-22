namespace OfficeAppointmentSoftware.Entities.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Works
    {
        public int WorksID { get; set; }

        [Required]
        [StringLength(300)]
        public string PhotoURL { get; set; }

        [Required]
        [StringLength(150)]
        public string Title { get; set; }

        [Required]
        [StringLength(300)]
        public string Description { get; set; }
    }
}
