namespace OfficeAppointmentSoftware.MVC.WebUI.Areas.Ofiice.Models
{
    public class ClientInfosDTO
    {
        public int ID { get; set; }
        public string DanisanTuru { get; set; }
        public int Yas { get; set; }
        public string TCKimlik { get; set; }
        public string AdSoyad { get; set; }
        public string Telno { get; set; }
        public string Email { get; set; }
        public string Adres { get; set; }
        public bool DevamDurumu { get; set; }
    }

    public class ClientDetailVM
    {
        public OfficeAppointmentSoftware.Entities.Entities.Client Client { get; set; }
        public OfficeAppointmentSoftware.Entities.Entities.Contact Contact { get; set; }
        public int Age { get; set; }
        public List<AgendaItemVM> Appointments { get; set; } = new();
    }
}
