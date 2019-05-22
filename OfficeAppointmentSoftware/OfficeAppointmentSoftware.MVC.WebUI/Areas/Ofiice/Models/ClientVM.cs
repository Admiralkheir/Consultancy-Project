using OfficeAppointmentSoftware.Entities.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OfficeAppointmentSoftware.MVC.WebUI.Areas.Ofiice.Models
{
    public class ClientVM
    {
         
        [AllowHtml]
        public Client Client { get; set; }
        public Contact Contact { get; set; }
    }
}