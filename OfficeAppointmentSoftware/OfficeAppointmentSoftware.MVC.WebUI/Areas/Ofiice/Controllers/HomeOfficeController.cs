using OfficeAppointmentSoftware.BLL.Concrete;
using OfficeAppointmentSoftware.BLL.Concrete.AdminServices;
using OfficeAppointmentSoftware.DAL.Contexts;
using OfficeAppointmentSoftware.Entities.Entities;
using OfficeAppointmentSoftware.MVC.WebUI.Areas.Ofiice.Models;
using OfficeAppointmentSoftware.MVC.WebUI.Controllers;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Core.Objects;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using static OfficeAppointmentSoftware.MVC.WebUI.Areas.Helper.FileHelper;

namespace OfficeAppointmentSoftware.MVC.WebUI.Areas.Ofiice.Controllers
{
    [Authorize(Roles = "User")]
    public class HomeOfficeController : Controller
    {
        ClientService _clientService;
        ExpertService _expertService;
        ContactService _contactService;
        ClientExpertVM clientsAndexperts = new ClientExpertVM();
        DDLsVM model = new DDLsVM();
        CompanyService _companyService;
        
        public HomeOfficeController()
        {
            _contactService = new ContactService();
            _companyService = new CompanyService();
            _clientService = new ClientService();
            _expertService = new ExpertService();
            model.ExpertsList = GetExperts;
            model.ClientsList = GetClients;
            //ViewBag.CompanyID = AccountController._compID ;
            clientsAndexperts.Company = _companyService.Find(x => x.CompanyID == AccountController._compID);
        }
        public ActionResult Index()
        {
            clientsAndexperts.ClientTotal = _ctx.Client.Where(x => x.Continuity == true).Where(x=>x.ClientID!=8 && x.CompanyID== AccountController._compID).Count();
            clientsAndexperts.ExpertsTotal = _ctx.Expert.Where(x=>x.Company.CompanyID == AccountController._compID).Count();
            clientsAndexperts.AppsTotal = _ctx.Appointment.Where(x => x.StartTime.Month == DateTime.Now.Month && x.Client.CompanyID == AccountController._compID).Count();
            
            clientsAndexperts.AppsTotalDaily = _ctx.Database.SqlQuery<Appointment>(
                "SELECT * FROM Appointment join Expert on Appointment.ExpertID=Expert.ExpertID WHERE DAY(StartTime) = DATEADD(day, 0, day(GetDate()))").ToList() ;
            clientsAndexperts.AppointmentsExpert = _ctx.Database.SqlQuery<Expert>(
    "SELECT * FROM Appointment join Expert on Appointment.ExpertID=Expert.ExpertID WHERE DAY(StartTime) = DATEADD(day, 0, day(GetDate())) ").ToList();
            clientsAndexperts.AppointmentsClient = _ctx.Database.SqlQuery<Client>(
"SELECT * FROM Appointment join Client on Appointment.ClientID=Client.ClientID WHERE DAY(StartTime) = DATEADD(day, 0, day(GetDate())) ").ToList();

            if (clientsAndexperts.ClientTotal!=0)
            {
            clientsAndexperts.AppsPercent = (Math.Truncate((Convert.ToDecimal(clientsAndexperts.AppsTotal) / 40) * 100));
            //clientsAndexperts.Kid1 = Math.Truncate(((((from c in _ctx.Client where (DateTime.Now.Year - c.BirthDate.Year) < 4 && c.ClientID!=8  select c).Count()) / Convert.ToDecimal(clientsAndexperts.ClientTotal))*100));
            clientsAndexperts.Kid2 = Math.Round((((from c in _ctx.Client/*.Where(x=>x.CompanyID==1)*/ where (DateTime.Now.Year - c.BirthDate.Year) < 13 && (DateTime.Now.Year - c.BirthDate.Year) > 2 && c.ClientID != 8 && c.CompanyID==AccountController._compID select c).Count()) / Convert.ToDecimal(clientsAndexperts.ClientTotal)) * 100,2);
            clientsAndexperts.Young = Math.Round((((from c in _ctx.Client where (DateTime.Now.Year - c.BirthDate.Year) > 11 && (DateTime.Now.Year - c.BirthDate.Year) < 18 && c.ClientID != 8 && c.CompanyID == AccountController._compID select c).Count() ) / Convert.ToDecimal(clientsAndexperts.ClientTotal)) * 100,2);
            clientsAndexperts.Adult = Math.Round((((from c in _ctx.Client where (DateTime.Now.Year - c.BirthDate.Year) > 17 && (DateTime.Now.Year - c.BirthDate.Year) < 40 && c.ClientID != 8 && c.CompanyID == AccountController._compID select c).Count()) / Convert.ToDecimal(clientsAndexperts.ClientTotal)) * 100,2);
            clientsAndexperts.Old = Math.Round((((from c in _ctx.Client where (DateTime.Now.Year - c.BirthDate.Year) >39 && c.ClientID != 8 && c.CompanyID == AccountController._compID select c).Count()) / Convert.ToDecimal(clientsAndexperts.ClientTotal)) * 100,2);

            }

            return View(clientsAndexperts);
        }
        [HttpGet]
        public ActionResult ExpertAdd()
        {
            return View();
        }
      
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult ExpertAdd(ExpertVM model, IEnumerable<HttpPostedFileBase> file)
        {

            ContactService _contactService = new ContactService();
            string physicalPath = " ~/Areas/Ofiice/image/";
            int maxFileSize = 500000;

            Expert expert;

            Dictionary<FileResultItem, FileResultType> resultModel = FileDocumentUpload(file, maxFileSize, physicalPath, new string[] { "image/gif", "image/png", "image/jpeg", "image/pjpeg", "image/bmp", "image/x-png", "image/jpg" });

            foreach (var item in resultModel)
            {
                if (item.Value == FileResultType.Error || item.Value == FileResultType.NoneFile || item.Value == FileResultType.SizeOver || item.Value == FileResultType.WrongType)
                {
                    RemoveAll(resultModel.Keys, physicalPath);
                    TempData["NoteCss"] = "warning";
                    TempData["NoteText"] = ControlMessages(item.Value, maxFileSize).Keys.FirstOrDefault().ToString();

                    return View("ExpertAdd");
                }
            }

            expert = new Expert();
            foreach (var item in resultModel.Keys)
            {
                try
                {
                expert.PhotoURL = item.UploadPath;
                expert.Name = model.Expert.Name;
                expert.Surname = model.Expert.Surname;
                expert.Profession = model.Expert.Profession;
                expert.WagePolicy = model.Expert.WagePolicy;
                expert.Note = model.Expert.Note;
                expert.BirthDate = model.Expert.BirthDate;
                expert.CompanyID =Convert.ToInt32(AccountController._compID);
                _expertService.AddExpert(expert);
                _contactService.AddContact(new Contact { ExpertID = expert.ExpertID, Address = model.Contact.Address, PhoneNumber1 = model.Contact.PhoneNumber1, PhoneNumber2 = model.Contact.PhoneNumber2, IsActive = true, Email = model.Contact.Email });
                    ViewBag.Mesaj = "Uzman Başarıyla Eklendi";

                }
                catch (Exception)
                {

                    throw;
                }  
            }
            return View("ExpertAdd");
        }
       
        [HttpGet]
        public ActionResult ClientAdd()
        {
            return View();
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult ClientAdd(ClientVM model)
        {
           
          //  JsonModel jmodel = new jsonModel();
            Client client;
            if (ModelState.IsValid)
            {
                try
                {
                    client = new Client();
                    client.Name = model.Client.Name;
                    client.Surname = model.Client.Surname;
                    client.TcKimlik = model.Client.TcKimlik;
                    client.Problem = model.Client.Problem;
                    client.BirthDate = model.Client.BirthDate;
                    client.Note = model.Client.Note;
                    client.Continuity = true;
                    client.CompanyID = Convert.ToInt32(AccountController._compID); ;
                    _clientService.AddClient(client);
                    _contactService.AddContact(new Contact { ClientID = client.ClientID, Address = model.Contact.Address, PhoneNumber1 = model.Contact.PhoneNumber1, PhoneNumber2 = model.Contact.PhoneNumber2, IsActive = true, Email = model.Contact.Email });

                    ViewBag.Message = "Danışan Başarıyla Eklendi";

                }
                catch (Exception)
                {

                    throw;
                }
            }
            return View();
        
        }
        public static int ConId;
       [HttpGet]
        public ActionResult ClientUpdate(int id)
        {
           

            ClientVM model = new ClientVM();
            model.Client = _clientService.FindClient(x => x.ClientID == id);
            model.Contact = _contactService.Find(x => x.ClientID == id && x.IsActive == true);
           ConId= model.Contact.ContactID;
            return View(model);
        }
      

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult ClientUpdate(ClientVM model)
        {
            ContactService oldContactService = new ContactService();

            Contact old = new Contact();
            old = oldContactService.Find(x => x.ContactID == ConId && x.IsActive == true);
            old.IsActive = false;
            oldContactService.UpdateContact(old);

            if (ModelState.IsValid)
            {
                try
                {
               _clientService.UpdateClient(model.Client);
                _contactService.AddContact(new Contact { ClientID = model.Client.ClientID, Address = model.Contact.Address, PhoneNumber1 = model.Contact.PhoneNumber1, PhoneNumber2 = model.Contact.PhoneNumber2, IsActive = true, Email = model.Contact.Email });
                    ViewBag.MessageUpdate = "Danışan Başarıyla Güncellendi";
               
            }
                
                catch (Exception)
                {

                    throw;
                }

            }   
            return RedirectToAction("ListClientInfos");

        }
        public static int ExpCOnId;
        [HttpGet]
        public ActionResult ExpertUpdate(short id)
        {

            ExpertVM model = new ExpertVM();
            model.Expert = _expertService.FindExpert(x => x.ExpertID == id);
            model.Contact = _contactService.Find(x => x.ExpertID == id && x.IsActive == true);
            ExpCOnId = model.Contact.ContactID;

            return View(model);

        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult ExpertUpdate(ExpertVM model, IEnumerable<HttpPostedFileBase> file)
        {
            ContactService oldService = new ContactService();
            Contact old = new Contact();
            old = oldService.Find(x => x.ContactID == ExpCOnId && x.IsActive == true);
            old.IsActive = false;
            oldService.UpdateContact(old);
      
                List<FileResultItem> fileResultItems = new List<FileResultItem> { new FileResultItem { UploadPath = model.Expert.PhotoURL } };
                //RemoveAll(fileResultItems, "~/Areas/Ofiice/image/");

                _expertService.UpdateExpert(model.Expert);
                _contactService.AddContact(new Contact { ExpertID = model.Expert.ExpertID, Address = model.Contact.Address, PhoneNumber1 = model.Contact.PhoneNumber1, PhoneNumber2 = model.Contact.PhoneNumber2, IsActive = true, Email = model.Contact.Email });
        
            return RedirectToAction("ListExpertInfos");

        }
        #region TAKVIM 

        public AppointmentDBContext _ctx = new AppointmentDBContext();

        public object JsonConvert { get; private set; }
        private IEnumerable<SelectListItem> GetClients
        {
            get
            {

                return _ctx.Client.Where(x => x.Continuity == true && x.CompanyID==AccountController._compID).Select(a => new SelectListItem
                {
                    Text = (a.Name + " " + a.Surname),
                    Value = a.ClientID.ToString(),
                    //  Selected = a.ClientID == a.Event.Select(x=>x.ClientID).FirstOrDefault()? true : false
                }).ToList();

            }
        }
        private IEnumerable<SelectListItem> GetExperts
        {
            get
            {
                return _ctx.Expert.Where(x=>x.CompanyID==AccountController._compID).Select(a => new SelectListItem
                {
                    Text = (a.Name + " " + a.Surname),
                    Value = a.ExpertID.ToString(),
                    //Selected = a.ExpertID == a.Event.Select(x => x.ExpertID).FirstOrDefault() ? true : false
                }).ToList();
            }
        }
        // GET: Home
        [HttpGet]
        public ActionResult AppointmentOpss()
        {
            return View(model);
        }
        [HttpPost]
        public JsonResult GetSelecteds(int selectedClientId, int selectedExpertId)
        {
            if (selectedClientId > 0 && selectedExpertId > 0)
            {
                model.SelectedExpertId = selectedExpertId;
                model.SelectedClientId = selectedClientId;
            }
            else
            {
                model.SelectedExpertId = 20;
                model.SelectedClientId = 19;
            }

            return Json(model, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetEvents()
        {
            _ctx.Configuration.ProxyCreationEnabled = false;
            var events = _ctx.Appointment.Where(x=>x.Expert.CompanyID==AccountController._compID).ToList();
            return Json(events, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult SaveEvent(Appointment data)
        {

            _ctx.Configuration.ProxyCreationEnabled = false;


            var status = false;

            if (data.AppointmentID > 0)
            {
                Appointment oldEvent = _ctx.Appointment.Where(x => x.AppointmentID == data.AppointmentID).FirstOrDefault();
                if (oldEvent != null)
                {

                    oldEvent.ExpertID = data.ExpertID;
                    oldEvent.ClientID = data.ClientID;
                    oldEvent.Subject = data.Subject;
                    oldEvent.Description = data.Description == null ? "" : data.Description;
                    oldEvent.ThemeColor = data.ThemeColor;
                    oldEvent.IsFullDay = data.IsFullDay;
                    oldEvent.EndTime = data.EndTime;
                    oldEvent.StartTime = data.StartTime;
                }
            }
            else
            {
                _ctx.Appointment.Add(data);
            }
            _ctx.SaveChanges();
            status = true;

            return new JsonResult { Data = new { status = status } };

        }

        [HttpPost]
        public JsonResult DeleteEvent(int id)
        {
            var status = false;

            using (AppointmentDBContext dc = new AppointmentDBContext())
            {
                Appointment deletedEvent = dc.Appointment.Where(x => x.AppointmentID == id).FirstOrDefault();
                if (deletedEvent != null)
                {
                    dc.Appointment.Remove(deletedEvent);
                    dc.SaveChanges();
                    status = true;
                }
            }
            return new JsonResult { Data = new { status = status } };
        }
        #endregion
        public ActionResult ExpertInfo()
        {
            return View();
        }
        [HttpGet]
        public ActionResult ListClientInfos()
        {
            using (AppointmentDBContext ctx = new AppointmentDBContext())
            {
                var ClientInfos = (from c in ctx.Client
                                   join co in ctx.Contact on c.ClientID equals co.ClientID
                                   where c.ClientID != 8 && co.IsActive == true && c.CompanyID==AccountController._compID
                                   select new ClientInfosDTO
                                   {
                                       ID = c.ClientID,
                                       DanisanTuru = (DateTime.Now.Year - c.BirthDate.Year) > 18 ? "Yetişkin" : "Çocuk",
                                       TCKimlik = c.TcKimlik,
                                       AdSoyad = c.Name+" "+c.Surname,
                                       //Soyad = c.Surname,
                                       Telno = co.PhoneNumber1,
                                       Adres = co.Address,
                                       DevamDurumu = c.Continuity,
                                   }).OrderBy(x=>x.AdSoyad).ToList();
                return View(ClientInfos);
            }


        }
        public ActionResult ListExpertInfos()
        {
            using (AppointmentDBContext ctx = new AppointmentDBContext())
            {
                var ExpertInfos = (from c in ctx.Expert
                                   join co in ctx.Contact on c.ExpertID equals co.ExpertID
                                   where c.ExpertID != 1 && co.IsActive == true && c.CompanyID==AccountController._compID

                                   select new ExpertInfosDTO
                                   {
                                       ID = c.ExpertID,
                                       Uzmanlik = c.Profession,
                                       AdSoyad = c.Name+" "+c.Surname ,
                                       //Soyad = c.Surname,
                                       Telno = co.PhoneNumber1,
                                       Not = c.Note
                                   }).OrderBy(x => x.AdSoyad).ToList();
                return View(ExpertInfos);
            }


        }
        [HttpGet]
        public ActionResult ClientDelete(int id)
        {
            _clientService.DeleteClient(id);
            return RedirectToAction("ListClientInfos");
        }
        [HttpGet]
        public ActionResult ExpertDelete(short id)
        {
            _expertService.DeleteExpert(id);
            return RedirectToAction("ListExpertInfos");
        }
    }
}
