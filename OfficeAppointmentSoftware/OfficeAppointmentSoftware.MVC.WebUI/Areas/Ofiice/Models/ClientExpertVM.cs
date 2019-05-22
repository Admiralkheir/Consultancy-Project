using OfficeAppointmentSoftware.Entities.Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Web;

namespace OfficeAppointmentSoftware.MVC.WebUI.Areas.Ofiice.Models
{
    public class ClientExpertVM
    {
        public int ClientTotal { get; set; }
        public int ExpertsTotal { get; set; }

        public int AppsTotal { get; set; }
        public IEnumerable<Appointment> AppsTotalDaily { get; set; }
        public IEnumerable<Expert> AppointmentsExpert { get; set; }
        public IEnumerable<Client> AppointmentsClient { get; set; }
        public decimal AppsPercent { get; set; }
        public decimal Kid2 { get; set; }
        public decimal Young { get; set; }
        public decimal Adult { get; set; }

        public decimal Old { get; set; }

        public Company Company { get; set; }
    }
}