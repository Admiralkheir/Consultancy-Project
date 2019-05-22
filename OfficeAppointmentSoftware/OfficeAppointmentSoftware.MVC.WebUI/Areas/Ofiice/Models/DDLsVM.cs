using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OfficeAppointmentSoftware.MVC.WebUI.Areas.Ofiice.Models
{
    public class DDLsVM
    {
        public int SelectedExpertId { get; set; }
        public int SelectedClientId { get; set; }
        public IEnumerable<SelectListItem> ExpertsList { get; set; }
        public IEnumerable<SelectListItem>  ClientsList { get; set; }
    }
}