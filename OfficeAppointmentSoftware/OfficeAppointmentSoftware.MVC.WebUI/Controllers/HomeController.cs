using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using OfficeAppointmentSoftware.BLL.Abstracts.AdminServices;
using OfficeAppointmentSoftware.MVC.WebUI.Models;
using OfficeAppointmentSoftware.MVC.WebUI.Models.Configurations;
using System.Net;
using System.Net.Mail;

namespace OfficeAppointmentSoftware.MVC.WebUI.Controllers
{
    public class HomeController : Controller
    {
        private readonly IFeaturesService _featuresService;
        private readonly IWorksService _worksService;
        private readonly IBannerService _bannerService;
        private readonly IPlanService _planService;
        private readonly SmtpSettings _smtp;
        private readonly ILogger<HomeController> _logger;

        public HomeController(IFeaturesService featuresService, IWorksService worksService, IBannerService bannerService,
            IPlanService planService, IOptions<SmtpSettings> smtp, ILogger<HomeController> logger)
        {
            _featuresService = featuresService;
            _worksService = worksService;
            _bannerService = bannerService;
            _planService = planService;
            _smtp = smtp.Value;
            _logger = logger;
        }

        // GET: Home
        public IActionResult Index()
        {
            return View(LoadContent(new UserContact()));
        }

        [HttpPost]
        public IActionResult EmailSend(UserContact model)
        {
            if (!ModelState.IsValid)
            {
                // Show the form again with its errors instead of silently redirecting.
                ViewBag.ScrollToContact = true;
                return View("Index", LoadContent(model));
            }

            // Credentials come from configuration ("Smtp" section) instead of being hard-coded.
            if (string.IsNullOrWhiteSpace(_smtp.Host) || string.IsNullOrWhiteSpace(_smtp.FromAddress) || string.IsNullOrWhiteSpace(_smtp.ToAddress))
            {
                TempData["Error"] = "E-posta gönderimi henüz yapılandırılmamış. Lütfen telefonla ulaşın.";
                return Redirect(Url.Action("Index") + "#contact");
            }

            try
            {
                var senderEmail = new MailAddress(_smtp.FromAddress, _smtp.FromName);
                var receiverEmail = new MailAddress(_smtp.ToAddress, _smtp.ToName);
                using var smtp = new SmtpClient
                {
                    Host = _smtp.Host,
                    Port = _smtp.Port,
                    EnableSsl = _smtp.EnableSsl,
                    DeliveryMethod = SmtpDeliveryMethod.Network,
                    UseDefaultCredentials = false,
                    Credentials = new NetworkCredential(_smtp.UserName ?? senderEmail.Address, _smtp.Password)
                };
                using var mess = new MailMessage(senderEmail, receiverEmail)
                {
                    Subject = model.Subject,
                    Body = model.Message + "\n\n" + model.NameSurname + "\n" + model.EMail
                };
                mess.ReplyToList.Add(new MailAddress(model.EMail, model.NameSurname));
                smtp.Send(mess);
                TempData["Success"] = "Mesajınız gönderildi. En kısa sürede dönüş yapacağız.";
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Contact form e-mail could not be sent.");
                TempData["Error"] = "Mesajınız gönderilemedi. Lütfen daha sonra tekrar deneyin.";
            }

            return Redirect(Url.Action("Index") + "#contact");
        }

        private UserContact LoadContent(UserContact model)
        {
            model.Features = _featuresService.GetFeatures().OrderBy(x => x.FeaturesID).ToList();
            model.Works = _worksService.GetWorks().OrderByDescending(x => x.WorksID).ToList();
            model.Banner = _bannerService.BannerGetDefault();
            model.Plans = _planService.GetPlans().OrderBy(x => x.Price).ToList();
            return model;
        }
    }
}
