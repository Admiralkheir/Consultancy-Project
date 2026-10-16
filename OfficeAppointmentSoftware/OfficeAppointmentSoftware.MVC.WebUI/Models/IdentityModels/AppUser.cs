using Microsoft.AspNetCore.Identity;

namespace OfficeAppointmentSoftware.MVC.WebUI.Models.IdentityModels
{
    public class AppUser : IdentityUser
    {
        public int? CompID { get; set; }

    }
}
