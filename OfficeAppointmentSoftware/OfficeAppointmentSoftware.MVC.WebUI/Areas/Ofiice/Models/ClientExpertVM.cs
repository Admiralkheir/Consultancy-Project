using OfficeAppointmentSoftware.Entities.Entities;

namespace OfficeAppointmentSoftware.MVC.WebUI.Areas.Ofiice.Models
{
    // Office dashboard.
    public class ClientExpertVM
    {
        public Company Company { get; set; }
        public int ClientTotal { get; set; }
        public int ExpertsTotal { get; set; }
        public int AppsToday { get; set; }
        public int AppsThisWeek { get; set; }
        public List<AgendaItemVM> TodaysAppointments { get; set; } = new();
        public List<AgendaItemVM> UpcomingAppointments { get; set; } = new();
        public List<AgeGroupVM> AgeGroups { get; set; } = new();
    }

    public class AgeGroupVM
    {
        public string Name { get; set; }
        public string Range { get; set; }
        public int Count { get; set; }
        public decimal Percent { get; set; }
    }

    // An appointment as shown in lists (dashboard, client/expert detail).
    public class AgendaItemVM
    {
        public int AppointmentID { get; set; }
        public string Subject { get; set; }
        public DateTime Start { get; set; }
        public DateTime? End { get; set; }
        public bool IsFullDay { get; set; }
        public string ExpertName { get; set; }
        public string ClientName { get; set; }
        public string Color { get; set; }
    }
}
