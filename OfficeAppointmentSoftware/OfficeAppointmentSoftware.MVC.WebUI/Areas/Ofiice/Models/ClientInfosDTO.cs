using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OfficeAppointmentSoftware.MVC.WebUI.Areas.Ofiice.Models
{
    public class ClientInfosDTO
    {
        public int ID { get; set; }
        public string DanisanTuru { get; set; }
        public string TCKimlik { get; set; }
        public string AdSoyad { get; set; }
        //public string Soyad { get; set; }
        public string Telno { get; set; }
        public string Adres { get; set; }
        public bool DevamDurumu { get; set; }
    }
}