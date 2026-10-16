using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using OfficeAppointmentSoftware.BLL.Abstracts;
using OfficeAppointmentSoftware.BLL.Abstracts.AdminServices;
using OfficeAppointmentSoftware.DAL.Contexts;
using OfficeAppointmentSoftware.Entities.Entities;
using OfficeAppointmentSoftware.MVC.WebUI.Areas.Admin.Models;
using OfficeAppointmentSoftware.MVC.WebUI.Models.IdentityModels;
using static OfficeAppointmentSoftware.MVC.WebUI.Areas.Helper.FileHelper;

namespace OfficeAppointmentSoftware.MVC.WebUI.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private const string ImagesPath = "~/Areas/Admin/Files/Images/";
        private const int MaxFileSize = 500000;
        private static readonly string[] ImageTypes = { "image/gif", "image/png", "image/jpeg", "image/pjpeg", "image/bmp", "image/x-png", "image/jpg", "image/webp" };

        private readonly IBannerService _bannerService;
        private readonly IFeaturesService _featuresService;
        private readonly IWorksService _worksService;
        private readonly IPlanService _planService;
        private readonly IContactService _contactService;
        private readonly ICompanyService _companyService;
        private readonly AppointmentDBContext _ctx;
        private readonly string _webRootPath;

        private readonly UserManager<AppUser> userManager;
        private readonly RoleManager<AppRole> roleManager;
        public AdminController(UserManager<AppUser> userManager, RoleManager<AppRole> roleManager,
            IFeaturesService featuresService, IBannerService bannerService, IWorksService worksService,
            IPlanService planService, IContactService contactService, ICompanyService companyService,
            AppointmentDBContext ctx, IWebHostEnvironment environment)
        {
            this.userManager = userManager;
            this.roleManager = roleManager;

            _featuresService = featuresService;
            _bannerService = bannerService;
            _worksService = worksService;
            _planService = planService;
            _contactService = contactService;
            _companyService = companyService;
            _ctx = ctx;
            _webRootPath = environment.WebRootPath;
        }

        private IEnumerable<SelectListItem> GetPlans
        {
            get
            {
                return _planService.GetPlans().OrderBy(a => a.Price).Select(a => new SelectListItem
                {
                    Text = a.PlanName + " — " + a.Price.ToString("N0") + " ₺",
                    Value = a.PlanID.ToString()
                }).ToList();
            }
        }

        #region Offices

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var users = await userManager.Users.Where(u => u.CompID != null).ToListAsync();
            var offices = _ctx.Company
                .Select(c => new OfficeListItemVM
                {
                    CompanyID = c.CompanyID,
                    CompanyName = c.CompanyName,
                    OwnerName = c.OwnerName,
                    LogoURL = c.LogoURL,
                    PlanName = c.Plans.PlanName,
                    Email = c.Contact.Where(x => x.IsActive).Select(x => x.Email).FirstOrDefault(),
                    Phone = c.Contact.Where(x => x.IsActive).Select(x => x.PhoneNumber1).FirstOrDefault(),
                    ExpertCount = c.Expert.Count(),
                    ClientCount = c.Client.Count(x => x.Continuity)
                })
                .OrderBy(x => x.CompanyName)
                .ToList();
            foreach (var office in offices)
            {
                office.UserName = users.FirstOrDefault(u => u.CompID == office.CompanyID)?.UserName;
                office.Phone = office.Phone?.Trim();
            }
            return View(offices);
        }

        [HttpGet]
        public IActionResult SignUp()
        {
            SignUpVM model = new SignUpVM();
            model.DropDownList = GetPlans;
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> SignUp(SignUpVM model, IEnumerable<IFormFile> file)
        {
            model.DropDownList = GetPlans;

            if (!string.IsNullOrWhiteSpace(model.Username) && await userManager.FindByNameAsync(model.Username) != null)
            {
                ModelState.AddModelError(nameof(model.Username), "Bu kullanıcı adı zaten kullanılıyor.");
            }
            if (!string.IsNullOrEmpty(model.Password))
            {
                foreach (var validator in userManager.PasswordValidators)
                {
                    var check = await validator.ValidateAsync(userManager, new AppUser(), model.Password);
                    foreach (var error in check.Errors)
                    {
                        ModelState.AddModelError(nameof(model.Password), error.Description);
                    }
                }
            }
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            // The logo is optional.
            string logo = null;
            if (file != null && file.Any())
            {
                var upload = FileDocumentUpload(file, MaxFileSize, ImagesPath, _webRootPath, ImageTypes);
                var failed = upload.FirstOrDefault(x => x.Value != FileResultType.Complete);
                if (failed.Key != null)
                {
                    RemoveAll(upload.Keys, ImagesPath, _webRootPath);
                    ModelState.AddModelError("file", ControlMessages(failed.Value, MaxFileSize).Keys.First());
                    return View(model);
                }
                logo = upload.Keys.First().UploadPath;
            }

            Company company = new Company
            {
                PlanID = Convert.ToInt16(model.SelectedPlanID),
                OwnerName = model.NewCompany.OwnerName,
                Description = model.NewCompany.Description,
                LogoURL = logo,
                CompanyName = model.NewCompany.CompanyName
            };
            _companyService.AddCompany(company);

            Contact contact = new Contact
            {
                CompanyID = company.CompanyID,
                Email = model.NewContact.Email,
                PhoneNumber1 = model.NewContact.PhoneNumber1,
                PhoneNumber2 = model.NewContact.PhoneNumber2,
                Address = model.NewContact.Address,
                IsActive = true
            };
            _contactService.AddContact(contact);

            if (!await roleManager.RoleExistsAsync("User"))
            {
                await roleManager.CreateAsync(new AppRole("User"));
            }
            AppUser user = new AppUser { Email = contact.Email, UserName = model.Username, CompID = company.CompanyID };
            IdentityResult result = await userManager.CreateAsync(user, model.Password);
            if (result.Succeeded)
            {
                await userManager.AddToRoleAsync(user, "User");
                TempData["Success"] = $"\"{company.CompanyName}\" ofisi oluşturuldu. Kullanıcı adı: {user.UserName}";
                return RedirectToAction("Index");
            }

            // Roll back, so a failed user does not leave an office without a login behind.
            _contactService.DeleteContact(contact.ContactID);
            _companyService.DeleteCompany(company.CompanyID);
            RemoveAll(new[] { new FileResultItem { UploadPath = logo } }, ImagesPath, _webRootPath);
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }
            return View(model);
        }

        #endregion

        #region Plans

        [HttpGet]
        public IActionResult Plans()
        {
            var plans = _ctx.Plans
                .Select(p => new PlanListItemVM { PlanID = p.PlanID, PlanName = p.PlanName, Price = p.Price, OfficeCount = p.Company.Count() })
                .OrderBy(p => p.Price)
                .ToList();
            return View(plans);
        }

        [HttpGet]
        public IActionResult PlanEdit(short? id)
        {
            if (id == null)
            {
                return View(new Plans());
            }
            var plan = _planService.GetPlan(id.Value);
            return plan == null ? NotFound() : View(plan);
        }

        [HttpPost]
        public IActionResult PlanEdit(Plans model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            if (model.PlanID == 0)
            {
                _planService.AddPlan(model);
                TempData["Success"] = "Paket eklendi.";
            }
            else
            {
                _planService.UpdatePlan(model);
                TempData["Success"] = "Paket güncellendi.";
            }
            return RedirectToAction("Plans");
        }

        [HttpPost]
        public IActionResult PlanDelete(short id)
        {
            if (_ctx.Company.Any(c => c.PlanID == id))
            {
                TempData["Error"] = "Bu paketi kullanan ofisler olduğu için silinemez.";
            }
            else if (_planService.GetPlan(id) != null)
            {
                _planService.DeletePlan(id);
                TempData["Success"] = "Paket silindi.";
            }
            return RedirectToAction("Plans");
        }

        #endregion

        #region Site content

        [HttpGet]
        public IActionResult Banner()
        {
            return View(_bannerService.BannerGetDefault() ?? new Banner());
        }

        [HttpPost]
        public IActionResult Banner(Banner model)
        {
            ModelState.Remove(nameof(model.PhotoURL));
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            model.PhotoURL = "-";
            if (model.BannerID == 0)
            {
                _bannerService.BannerAdd(model);
            }
            else
            {
                _bannerService.BannerUpdate(model);
            }
            TempData["Success"] = "Karşılama alanı güncellendi.";
            return RedirectToAction("Banner");
        }

        [HttpGet]
        public IActionResult FeaturesList()
        {
            return View(_featuresService.GetFeatures().OrderBy(x => x.FeaturesID).ToList());
        }

        [HttpGet]
        public IActionResult FeaturesAdd()
        {
            return View("FeaturesUpdate", new Features());
        }

        [HttpGet]
        public IActionResult FeaturesUpdate(short id)
        {
            var feature = _featuresService.GetFeatures(x => x.FeaturesID == id).FirstOrDefault();
            return feature == null ? NotFound() : View(feature);
        }

        [HttpPost]
        public IActionResult FeaturesUpdate(Features model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            if (model.FeaturesID == 0)
            {
                _featuresService.AddFeatures(model);
                TempData["Success"] = "Özellik eklendi.";
            }
            else
            {
                _featuresService.UpdateFeatures(model);
                TempData["Success"] = "Özellik güncellendi.";
            }
            return RedirectToAction("FeaturesList");
        }

        [HttpPost]
        public IActionResult FeaturesDelete(short id)
        {
            if (_featuresService.GetFeatures(x => x.FeaturesID == id).Any())
            {
                _featuresService.DeleteFeatures(id);
                TempData["Success"] = "Özellik silindi.";
            }
            return RedirectToAction("FeaturesList");
        }

        [HttpGet]
        public IActionResult WorksList()
        {
            return View(_worksService.GetWorks().OrderByDescending(x => x.WorksID).ToList());
        }

        [HttpGet]
        public IActionResult WorksAdd()
        {
            return View("WorksUpdate", new Works());
        }

        [HttpGet]
        public IActionResult WorksUpdate(int id)
        {
            var work = _worksService.GetWorks(x => x.WorksID == id).FirstOrDefault();
            return work == null ? NotFound() : View(work);
        }

        [HttpPost]
        public IActionResult WorksUpdate(Works model, IEnumerable<IFormFile> file)
        {
            ModelState.Remove(nameof(model.PhotoURL));
            var hasFile = file != null && file.Any();
            if (model.WorksID == 0 && !hasFile)
            {
                ModelState.AddModelError("file", "Lütfen bir görsel seçiniz.");
            }
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            // A new image replaces the old one; otherwise the current one is kept (posted as a hidden field).
            if (hasFile)
            {
                var upload = FileDocumentUpload(file, MaxFileSize, ImagesPath, _webRootPath, ImageTypes);
                var failed = upload.FirstOrDefault(x => x.Value != FileResultType.Complete);
                if (failed.Key != null)
                {
                    RemoveAll(upload.Keys, ImagesPath, _webRootPath);
                    ModelState.AddModelError("file", ControlMessages(failed.Value, MaxFileSize).Keys.First());
                    return View(model);
                }
                if (model.WorksID != 0)
                {
                    RemoveAll(new[] { new FileResultItem { UploadPath = model.PhotoURL } }, ImagesPath, _webRootPath);
                }
                model.PhotoURL = upload.Keys.First().UploadPath;
            }

            if (model.WorksID == 0)
            {
                _worksService.AddWork(model);
                TempData["Success"] = "Çalışma eklendi.";
            }
            else
            {
                _worksService.Update(model);
                TempData["Success"] = "Çalışma güncellendi.";
            }
            return RedirectToAction("WorksList");
        }

        [HttpPost]
        public IActionResult WorksDelete(int id)
        {
            var work = _worksService.GetWorks(x => x.WorksID == id).FirstOrDefault();
            if (work != null)
            {
                _worksService.DeleteWork(id);
                RemoveAll(new[] { new FileResultItem { UploadPath = work.PhotoURL } }, ImagesPath, _webRootPath);
                TempData["Success"] = "Çalışma silindi.";
            }
            return RedirectToAction("WorksList");
        }

        #endregion
    }
}
