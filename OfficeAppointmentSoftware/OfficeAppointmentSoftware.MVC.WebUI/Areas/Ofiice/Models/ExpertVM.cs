using OfficeAppointmentSoftware.Entities.Entities;

namespace OfficeAppointmentSoftware.MVC.WebUI.Areas.Ofiice.Models
{
    // Add/edit form for an expert and its active contact details.
    public class ExpertVM
    {
        public Expert Expert { get; set; } = new Expert();
        public Contact Contact { get; set; } = new Contact();
    }
}
