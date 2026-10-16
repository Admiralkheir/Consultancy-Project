namespace OfficeAppointmentSoftware.Entities.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("Banner")]
    public partial class Banner
    {
        public short BannerID { get; set; }

        [Required]
        [StringLength(300)]
        public string PhotoURL { get; set; }

        [Required]
        [StringLength(100)]
        [Display(Name = "Başlık")]
        public string Title { get; set; }

        [Required]
        [StringLength(300)]
        [Display(Name = "Açıklama")]
        public string Description { get; set; }
    }
}
