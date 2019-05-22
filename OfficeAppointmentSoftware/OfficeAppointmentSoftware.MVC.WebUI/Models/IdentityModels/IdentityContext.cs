using Microsoft.AspNet.Identity.EntityFramework;
using OfficeAppointmentSoftware.MVC.WebUI.Models.Configurations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OfficeAppointmentSoftware.MVC.WebUI.Models.IdentityModels
{
    public class IdentityContext : IdentityDbContext<AppUser>
    {
        public IdentityContext() : base("AppointmentDBContext")
        {
           // Database.Connection.ConnectionString = @"server=BETUL-WINDOWS;Database=AppointmentDBson;UID=sa; PWD=12345";
            System.Data.Entity.Database.SetInitializer(new IdentityContextInitializer());
        }

        public static IdentityContext Create()
        {
            return new IdentityContext();
        }
    }
}