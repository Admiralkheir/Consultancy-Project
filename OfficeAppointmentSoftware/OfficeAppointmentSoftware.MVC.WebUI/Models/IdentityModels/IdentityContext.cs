using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace OfficeAppointmentSoftware.MVC.WebUI.Models.IdentityModels
{
    // Shares the AppointmentDB database with AppointmentDBContext; each context has its own migrations.
    public class IdentityContext : IdentityDbContext<AppUser, AppRole, string>
    {
        public IdentityContext(DbContextOptions<IdentityContext> options) : base(options)
        {
        }
    }
}
