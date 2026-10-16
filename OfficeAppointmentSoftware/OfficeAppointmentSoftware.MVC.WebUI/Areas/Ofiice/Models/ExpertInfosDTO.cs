namespace OfficeAppointmentSoftware.MVC.WebUI.Areas.Ofiice.Models
{
    public class ExpertInfosDTO
    {
        public short ID { get; set; }
        public string AdSoyad { get; set; }
        public string PhotoURL { get; set; }
        public string Telno { get; set; }
        public string Email { get; set; }
        public string Uzmanlik { get; set; }
        public string Not { get; set; }
        public int UpcomingCount { get; set; }
    }

    public class ExpertDetailVM
    {
        public OfficeAppointmentSoftware.Entities.Entities.Expert Expert { get; set; }
        public OfficeAppointmentSoftware.Entities.Entities.Contact Contact { get; set; }
        public int TotalAppointments { get; set; }
        public List<AgendaItemVM> UpcomingAppointments { get; set; } = new();
    }
}
