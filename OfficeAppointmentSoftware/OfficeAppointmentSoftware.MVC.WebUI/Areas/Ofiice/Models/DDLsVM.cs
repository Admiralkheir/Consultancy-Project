using Microsoft.AspNetCore.Mvc.Rendering;

namespace OfficeAppointmentSoftware.MVC.WebUI.Areas.Ofiice.Models
{
    // Calendar page: the office's experts and active clients for the appointment form.
    public class DDLsVM
    {
        public int SelectedExpertId { get; set; }
        public int SelectedClientId { get; set; }
        public IEnumerable<SelectListItem> ExpertsList { get; set; }
        public IEnumerable<SelectListItem> ClientsList { get; set; }
    }

    // Posted by the calendar when an appointment is created, edited, moved or resized.
    public class AppointmentInput
    {
        public int AppointmentID { get; set; }
        public short? ExpertID { get; set; }
        public int? ClientID { get; set; }
        public string Subject { get; set; }
        public string Description { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime? EndTime { get; set; }
        public string ThemeColor { get; set; }
        public bool IsFullDay { get; set; }
    }

    public static class AppointmentColors
    {
        // Value is stored in Appointment.ThemeColor and used as the calendar event colour.
        public static readonly (string Value, string Name)[] All =
        {
            ("#2563eb", "Mavi"),
            ("#0d9488", "Turkuaz"),
            ("#16a34a", "Yeşil"),
            ("#ca8a04", "Hardal"),
            ("#ea580c", "Turuncu"),
            ("#dc2626", "Kırmızı"),
            ("#db2777", "Pembe"),
            ("#7c3aed", "Mor"),
            ("#64748b", "Gri")
        };

        public const string Default = "#2563eb";
    }
}
