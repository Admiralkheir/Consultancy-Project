using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OfficeAppointmentSoftware.MVC.WebUI.Models.IdentityModels
{
    public class AppUser : IdentityUser
    {
        public int? CompID { get; set; }

    }
}