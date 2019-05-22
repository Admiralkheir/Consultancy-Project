using OfficeAppointmentSoftware.Entities.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OfficeAppointmentSoftware.MVC.WebUI.Areas.Ofiice.Models
{
    public class ExpertVM
    {
        [AllowHtml]
        public Expert Expert { get; set; }
        public Contact Contact { get; set; }
    }
}