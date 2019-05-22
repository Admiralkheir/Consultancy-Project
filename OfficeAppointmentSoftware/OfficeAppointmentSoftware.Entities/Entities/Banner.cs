namespace OfficeAppointmentSoftware.Entities.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Banner")]
    public partial class Banner
    {
        public short BannerID { get; set; }

        [Required]
        [StringLength(300)]
        public string PhotoURL { get; set; }

        [Required]
        [StringLength(100)]
        public string Title { get; set; }

        [Required]
        [StringLength(300)]
        public string Description { get; set; }
    }
}
