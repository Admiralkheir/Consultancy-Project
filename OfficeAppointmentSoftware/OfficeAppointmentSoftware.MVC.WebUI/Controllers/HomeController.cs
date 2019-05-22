using OfficeAppointmentSoftware.BLL.Concrete.AdminServices;
using OfficeAppointmentSoftware.MVC.WebUI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;
using System.Web.Mvc;

namespace OfficeAppointmentSoftware.MVC.WebUI.Controllers
{
    public class HomeController : Controller
    {
        // GET: Home
        public ActionResult Index()
        {
            UserContact dynamics = new UserContact();
            FeaturesService _featuresService = new FeaturesService();
            WorksService _worksService = new WorksService();
            BannerService _bannerService = new BannerService();
            dynamics.Features = _featuresService.GetFeatures().ToList();
            dynamics.Works = _worksService.GetWorks().ToList();
            dynamics.Banner = _bannerService.BannerGet(5);
            TempData["BannerTitle"] = dynamics.Banner.Title;
            TempData["BannerD"] = dynamics.Banner.Description;


            return View(dynamics);
        }
        [HttpPost]
        public ActionResult EmailSend(UserContact model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var senderEmail = new MailAddress("mahmutcaglayan1313@gmail.com", "Mahmut Çağlayan");
                    var receiverEmail = new MailAddress("tugay.ersoy@gmail.com", "Tugay Ersoy");
                    var password = "Brad1313";
                    var sub = model.Subject;
                    var body = model.Message +"\n"+model.EMail+"\n"+model.NameSurname;
                    var smtp = new SmtpClient
                    {
                        Host = "smtp.gmail.com",
                        Port = 587,
                        EnableSsl = true,
                        DeliveryMethod = SmtpDeliveryMethod.Network,
                        UseDefaultCredentials = false,
                        Credentials = new NetworkCredential(senderEmail.Address, password)
                    };
                    using (var mess = new MailMessage(senderEmail, receiverEmail)
                    {
                        Subject = sub,
                        Body = body
                    })
                    {
                        smtp.Send(mess);
                    }
                    return View();
                }
            }
            catch (Exception ex)
            {
                TempData["Mesaj"] = ex.Message;
            }

            return RedirectToAction("Index");
        }
    }
}