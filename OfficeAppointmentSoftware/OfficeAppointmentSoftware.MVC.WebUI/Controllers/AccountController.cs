using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using OfficeAppointmentSoftware.MVC.WebUI.Models;
using OfficeAppointmentSoftware.MVC.WebUI.Models.IdentityModels;

namespace OfficeAppointmentSoftware.MVC.WebUI.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<AppUser> userManager;
        private readonly SignInManager<AppUser> signInManager;
        public AccountController(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
        }

        [HttpGet]
        public IActionResult Login(string returnUrl = null)
        {
            // Signed-in users (also those sent here by "access denied") go to their own panel.
            if (User.Identity?.IsAuthenticated == true)
            {
                return RedirectToHome();
            }
            ViewBag.ReturnUrl = returnUrl;
            return View(new LoginVM());
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginVM model, string returnUrl = null)
        {
            ViewBag.ReturnUrl = returnUrl;
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            AppUser user = await userManager.FindByNameAsync(model.UserName);
            if (user != null)
            {
                // The company id travels in the auth cookie as a claim (see AppUserClaimsPrincipalFactory).
                var result = await signInManager.PasswordSignInAsync(user, model.Password, model.RememberMe, lockoutOnFailure: false);
                if (result.Succeeded)
                {
                    if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
                    {
                        return LocalRedirect(returnUrl);
                    }
                    return RedirectToHome(user);
                }
            }

            ModelState.AddModelError(string.Empty, "Kullanıcı adı veya şifre hatalı.");
            model.Password = null;
            return View(model);
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Logout()
        {
            await signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }

        private IActionResult RedirectToHome(AppUser user = null)
        {
            var isAdmin = user != null ? user.CompID == null : User.IsInRole("Admin");
            return isAdmin
                ? RedirectToAction("Index", "Admin", new { area = "Admin" })
                : RedirectToAction("Index", "HomeOffice", new { area = "Ofiice" });
        }
    }
}
