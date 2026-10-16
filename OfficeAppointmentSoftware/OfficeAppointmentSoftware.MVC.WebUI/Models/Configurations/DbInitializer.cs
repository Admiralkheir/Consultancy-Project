using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using OfficeAppointmentSoftware.DAL.Contexts;
using OfficeAppointmentSoftware.Entities.Entities;
using OfficeAppointmentSoftware.MVC.WebUI.Models.IdentityModels;

namespace OfficeAppointmentSoftware.MVC.WebUI.Models.Configurations
{
    // Replaces the EF6 initializers: applies migrations for both contexts and seeds the roles.
    // The "Seed" config section can also create an admin user and a little sample data.
    public static class DbInitializer
    {
        public static async Task InitializeAsync(IServiceProvider services, IConfiguration configuration, ILogger logger)
        {
            using var scope = services.CreateScope();
            var provider = scope.ServiceProvider;

            await MigrateWithRetryAsync(provider.GetRequiredService<AppointmentDBContext>(), logger);
            await MigrateWithRetryAsync(provider.GetRequiredService<IdentityContext>(), logger);

            var roleManager = provider.GetRequiredService<RoleManager<AppRole>>();
            foreach (var roleName in new[] { "Admin", "User" })
            {
                if (!await roleManager.RoleExistsAsync(roleName))
                {
                    await roleManager.CreateAsync(new AppRole(roleName));
                }
            }

            var seed = configuration.GetSection("Seed");
            var adminUserName = seed["AdminUserName"];
            var adminPassword = seed["AdminPassword"];
            if (!string.IsNullOrWhiteSpace(adminUserName) && !string.IsNullOrWhiteSpace(adminPassword))
            {
                var userManager = provider.GetRequiredService<UserManager<AppUser>>();
                if (await userManager.FindByNameAsync(adminUserName) == null)
                {
                    var admin = new AppUser { UserName = adminUserName };
                    var result = await userManager.CreateAsync(admin, adminPassword);
                    if (result.Succeeded)
                    {
                        await userManager.AddToRoleAsync(admin, "Admin");
                        logger.LogInformation("Seeded admin user {UserName}.", adminUserName);
                    }
                    else
                    {
                        logger.LogWarning("Could not seed admin user: {Errors}", string.Join("; ", result.Errors.Select(e => e.Description)));
                    }
                }
            }

            if (seed.GetValue<bool>("SampleData"))
            {
                var db = provider.GetRequiredService<AppointmentDBContext>();
                if (!await db.Plans.AnyAsync())
                {
                    db.Plans.AddRange(
                        new Plans { PlanName = "Temel", Price = 100 },
                        new Plans { PlanName = "Standart", Price = 250 },
                        new Plans { PlanName = "Premium", Price = 500 });
                }
                if (!await db.Banner.AnyAsync())
                {
                    db.Banner.Add(new Banner { PhotoURL = "-", Title = "CETRIS Randevu Sistemi", Description = "Danışmanlık merkezleri için randevu ve danışan yönetimi." });
                }
                await db.SaveChangesAsync();
            }
        }

        // SQL Server in docker-compose can still be starting up when the app boots.
        private static async Task MigrateWithRetryAsync(DbContext context, ILogger logger)
        {
            for (var attempt = 1; ; attempt++)
            {
                try
                {
                    await context.Database.MigrateAsync();
                    return;
                }
                catch (Exception ex) when (attempt < 10 && ex is Microsoft.Data.SqlClient.SqlException)
                {
                    logger.LogWarning("Database not reachable yet (attempt {Attempt}): {Message}", attempt, ex.Message);
                    await Task.Delay(TimeSpan.FromSeconds(5));
                }
            }
        }
    }
}
