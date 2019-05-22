using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Owin.Security;
using OfficeAppointmentSoftware.MVC.WebUI.Models;
using OfficeAppointmentSoftware.MVC.WebUI.Models.IdentityModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Web;
using System.Web.Mvc;

namespace OfficeAppointmentSoftware.MVC.WebUI.Controllers
{
    public class AccountController : Controller
    {
        public static int? _compID;
        private UserManager<AppUser> userManager;
        private RoleManager<AppRole> roleManager;
        public AccountController()
        {
            IdentityContext db = new IdentityContext();
            UserStore<AppUser> userStore = new UserStore<AppUser>(db);//bir identity tablosu user ile ilgili işlemler
            userManager = new UserManager<AppUser>(userStore);
            RoleStore<AppRole> roleStore = new RoleStore<AppRole>(db);
            roleManager = new RoleManager<AppRole>(roleStore);
        }
        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(LoginVM model)
        {
            AppUser user = userManager.Find(model.UserName, model.Password);
            if (user != null)
            {
                _compID=user.CompID;
                IAuthenticationManager authManager = HttpContext.GetOwinContext().Authentication;
                ClaimsIdentity identity = userManager.CreateIdentity(user, "ApplicationCookie");
                AuthenticationProperties authProps = new AuthenticationProperties();
                authManager.SignIn(authProps, identity);
                if (user.CompID==null)
                {
                    return RedirectToAction("Banner", "Admin/Admin");
                }
                else
                {
                    return RedirectToAction("Index", "Ofiice/HomeOffice");
                }
            }


            return View();
        }

    }
}