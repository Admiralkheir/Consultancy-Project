using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;

namespace OfficeAppointmentSoftware.MVC.WebUI.Models.IdentityModels
{
    // Puts the user's company id into the auth cookie, so each request knows its own office
    // (this replaces the old static AccountController._compID, which was shared by all users).
    public class AppUserClaimsPrincipalFactory : UserClaimsPrincipalFactory<AppUser, AppRole>
    {
        public const string CompanyIdClaimType = "CompID";

        public AppUserClaimsPrincipalFactory(UserManager<AppUser> userManager, RoleManager<AppRole> roleManager, IOptions<IdentityOptions> options)
            : base(userManager, roleManager, options)
        {
        }

        protected override async Task<ClaimsIdentity> GenerateClaimsAsync(AppUser user)
        {
            var identity = await base.GenerateClaimsAsync(user);
            if (user.CompID.HasValue)
            {
                identity.AddClaim(new Claim(CompanyIdClaimType, user.CompID.Value.ToString()));
            }
            return identity;
        }
    }

    public static class ClaimsPrincipalExtensions
    {
        public static int? GetCompanyId(this ClaimsPrincipal user)
        {
            return int.TryParse(user.FindFirstValue(AppUserClaimsPrincipalFactory.CompanyIdClaimType), out var id) ? id : null;
        }
    }
}
