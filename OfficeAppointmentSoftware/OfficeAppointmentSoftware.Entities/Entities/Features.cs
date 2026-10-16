namespace OfficeAppointmentSoftware.Entities.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public partial class Features
    {
        public short FeaturesID { get; set; }

        [Required]
        [StringLength(300)]
        [Display(Name = "Genel Açıklama")]
        public string GeneralDescription { get; set; }

        [Required]
        [StringLength(150)]
        [Display(Name = "Başlık")]
        public string Title { get; set; }

        [Required]
        [StringLength(300)]
        [Display(Name = "Açıklama")]
        public string Description { get; set; }
    }
}
