using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OfficeAppointmentSoftware.MVC.WebUI.Areas.Ofiice.Models
{
    public class ExpertInfosDTO
    {
        public short ID { get; set; }
        public string AdSoyad { get; set; }
        //public string Soyad { get; set; }
        public string Telno { get; set; }
        public string Uzmanlik { get; set; }
        public string Not { get; set; }
    }
}