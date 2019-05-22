using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using OfficeAppointmentSoftware.BLL.Concrete;
using OfficeAppointmentSoftware.BLL.Concrete.AdminServices;
using OfficeAppointmentSoftware.Entities.Entities;
using OfficeAppointmentSoftware.MVC.WebUI.Areas.Admin.Models;
using OfficeAppointmentSoftware.MVC.WebUI.Models.IdentityModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using static OfficeAppointmentSoftware.MVC.WebUI.Areas.Helper.FileHelper;

namespace OfficeAppointmentSoftware.MVC.WebUI.Areas.Admin.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        BannerService _bannerService;
        FeaturesService _featuresService;
        WorksService _worksService;
        PlanService _planService;
        ContactService _contactService;
        CompanyService _companyService;

        private UserManager<AppUser> userManager;
        private RoleManager<AppRole> roleManager;
        public static IdentityResult iResult;
        public AdminController()
        {
            IdentityContext db = new IdentityContext();
            UserStore<AppUser> userStore = new UserStore<AppUser>(db);//bir identity tablosu user ile ilgili işlemler
            userManager = new UserManager<AppUser>(userStore);
            RoleStore<AppRole> roleStore = new RoleStore<AppRole>(db);
            roleManager = new RoleManager<AppRole>(roleStore);

            _featuresService = new FeaturesService();
            _bannerService = new BannerService();
            _worksService = new WorksService();
            _planService = new PlanService();
            _contactService = new ContactService();
            _companyService = new CompanyService();
        }
        private IEnumerable<SelectListItem> GetPlans
        {
            get
            {
                return _planService.GetPlans().Select(a => new SelectListItem
                {
                    Text = (a.PlanName + " " + a.Price),
                    Value = a.PlanID.ToString()

                }
                ).ToList();
            }
        }
        // GET: Admin/Admin
        [HttpGet]
        public ActionResult SignUp()
        {
            SignUpVM model = new SignUpVM();
            model.DropDownList = GetPlans;
            return View(model);
        }

        [HttpPost]
        public ActionResult SignUp(SignUpVM model, IEnumerable<HttpPostedFileBase> file)
        {
            string physicalPath = "~/Areas/Admin/Files/Images/";
            int maxFileSize = 500000;
            Dictionary<FileResultItem, FileResultType> resultModel = FileDocumentUpload(file, maxFileSize, physicalPath, new string[] { "image/gif", "image/png", "image/jpeg", "image/pjpeg", "image/bmp", "image/x-png", "image/jpg" });
            foreach (var item in resultModel)
            {
                if (item.Value == FileResultType.Error || item.Value == FileResultType.NoneFile || item.Value == FileResultType.SizeOver || item.Value == FileResultType.WrongType)
                {
                    RemoveAll(resultModel.Keys, physicalPath);
                    //return View("SignUp");
                }
            }
            Company _company = new Company();
            foreach (var item in resultModel.Keys)
            {
                _company.PlanID = Convert.ToInt16(model.SelectedPlanID);

                _company.OwnerName = model.NewCompany.OwnerName;
                _company.Description = model.NewCompany.Description;
                _company.LogoURL = item.UploadPath;
                _company.CompanyName = model.NewCompany.CompanyName;
                _companyService.AddCompany(_company);
            }

            Contact _contact = new Contact();
            _contact.CompanyID = _company.CompanyID;
            _contact.Email = model.NewContact.Email;
            _contact.PhoneNumber1 = model.NewContact.PhoneNumber1;
            _contact.PhoneNumber2 = model.NewContact.PhoneNumber2;
            _contact.Address = model.NewContact.Address;
            _contact.IsActive = true;
            _contactService.AddContact(_contact);

            AppUser user = new AppUser();
            user.Email = model.NewContact.Email;
            user.Email = _contact.Email;
            user.UserName = model.Username;
            user.CompID = _company.CompanyID;
            if (!roleManager.RoleExists("User"))
            {
                iResult = roleManager.Create(new AppRole("User"));
            }
            iResult = userManager.Create(user, model.Password);

            if (iResult.Succeeded)
            {
                userManager.AddToRole(user.Id, "User");
                 return RedirectToAction("SignUp");
        }
            else
            {
                var errors = iResult.Errors;
                TempData["NoteError"] = errors.ToString();
                //ModelState.AddModelError("RegisterUser", "Kullanıcı ekleme işleminde hata!");
            }
            return View();
        }

        [HttpGet]
        public ActionResult Banner()
        {

            return View(_bannerService.FindById(1));
        }

        [HttpPost]
        public ActionResult Banner(Banner model)
        {
            try
            {
                model.PhotoURL = "-";
                _bannerService.BannerUpdate(model);
                return View();
            }
            catch (Exception)
            {

                return View();
            }

        }
        [HttpGet]
        public ActionResult FeaturesAdd()
        {

            return View();
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult FeaturesAdd(Features model)
        {
            try
            {
                _featuresService.AddFeatures(model);
                return View();
            }
            catch (Exception)
            {

                return View();
            }
        }

        [HttpGet]
        public ActionResult FeaturesList()
        {
            return View(_featuresService.GetFeatures());
        }

        [HttpGet]
        public ActionResult FeaturesDelete(short id)
        {
            _featuresService.DeleteFeatures(id);
            return RedirectToAction("FeaturesList");
        }
        [HttpGet]
        public ActionResult FeaturesUpdate(short id)
        {

            return View(_featuresService.GetFeatures(x => x.FeaturesID == id).FirstOrDefault());
        }
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult FeaturesUpdate(Features model)
        {
            _featuresService.UpdateFeatures(model);
            return RedirectToAction("FeaturesList");
        }

        [HttpGet]
        public ActionResult WorksList()
        {

            return View(_worksService.GetWorks());
        }

        [HttpGet]
        public ActionResult WorksDelete(short id)
        {
            _worksService.DeleteWork(id);
            return RedirectToAction("WorksList");
        }

        [HttpGet]
        public ActionResult WorksUpdate(short id)
        {

            return View(_worksService.GetWorks(x => x.WorksID == id).FirstOrDefault());

        }
        [HttpPost]
        public ActionResult WorksUpdate(Works model)
        {
            List<FileResultItem> fileResultItems = new List<FileResultItem> { new FileResultItem { UploadPath = model.PhotoURL } };
            RemoveAll(fileResultItems, "~/Areas/Admin/Files/Images/");

            _worksService.Update(model);
            return RedirectToAction("WorksList");

        }

        [HttpGet]
        public ActionResult WorksAdd()
        {

            return View();
        }

        [HttpPost]
        public ActionResult WorksAdd(Works model, IEnumerable<HttpPostedFileBase> file)
        {
            try
            {
                string physicalPath = "~/Areas/Admin/Files/Images/";
                int maxFileSize = 500000;

                Dictionary<FileResultItem, FileResultType> resultModel = FileDocumentUpload(file, maxFileSize, physicalPath, new string[] { "image/gif", "image/png", "image/jpeg", "image/pjpeg", "image/bmp", "image/x-png", "image/jpg" });

                foreach (var item in resultModel)
                {
                    if (item.Value == FileResultType.Error || item.Value == FileResultType.NoneFile || item.Value == FileResultType.SizeOver || item.Value == FileResultType.WrongType)
                    {
                        RemoveAll(resultModel.Keys, physicalPath);
                        return View("WorksAdd");
                    }
                }
                foreach (var item in resultModel.Keys)
                {
                    _worksService.Add(new Works { PhotoURL = item.UploadPath, Description = model.Description, Title = model.Title });
                }
            }
            catch (Exception)
            {
                throw;
            }
            return RedirectToAction("WorksList");
        }
    }
}