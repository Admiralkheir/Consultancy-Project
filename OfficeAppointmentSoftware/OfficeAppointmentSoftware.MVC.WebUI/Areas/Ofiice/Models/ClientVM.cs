using OfficeAppointmentSoftware.Entities.Entities;

namespace OfficeAppointmentSoftware.MVC.WebUI.Areas.Ofiice.Models
{
    // Add/edit form for a client and its active contact details.
    public class ClientVM
    {
        public Client Client { get; set; } = new Client { Continuity = true };
        public Contact Contact { get; set; } = new Contact();
    }
}
