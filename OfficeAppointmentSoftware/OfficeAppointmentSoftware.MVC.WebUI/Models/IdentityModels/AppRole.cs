using Microsoft.AspNetCore.Identity;

namespace OfficeAppointmentSoftware.MVC.WebUI.Models.IdentityModels
{
    public class AppRole : IdentityRole
    {
        public AppRole()
        {

        }
        public AppRole(string roleName) : base(roleName)
        {

        }

    }
}
