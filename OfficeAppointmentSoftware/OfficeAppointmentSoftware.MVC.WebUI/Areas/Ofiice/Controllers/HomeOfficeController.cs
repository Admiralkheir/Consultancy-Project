using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using OfficeAppointmentSoftware.BLL.Abstracts;
using OfficeAppointmentSoftware.BLL.Abstracts.AdminServices;
using OfficeAppointmentSoftware.DAL.Contexts;
using OfficeAppointmentSoftware.Entities.Entities;
using OfficeAppointmentSoftware.MVC.WebUI.Areas.Ofiice.Models;
using OfficeAppointmentSoftware.MVC.WebUI.Infrastructure;
using OfficeAppointmentSoftware.MVC.WebUI.Models.IdentityModels;
using static OfficeAppointmentSoftware.MVC.WebUI.Areas.Helper.FileHelper;

namespace OfficeAppointmentSoftware.MVC.WebUI.Areas.Ofiice.Controllers
{
    [Area("Ofiice")]
    [Authorize(Roles = "User")]
    public class HomeOfficeController : Controller
    {
        private const string ExpertImagesPath = "~/Areas/Ofiice/image/";
        private const int MaxFileSize = 500000;
        private static readonly string[] ImageTypes = { "image/gif", "image/png", "image/jpeg", "image/pjpeg", "image/bmp", "image/x-png", "image/jpg", "image/webp" };

        private readonly IClientService _clientService;
        private readonly IExpertService _expertService;
        private readonly IContactService _contactService;
        private readonly ICompanyService _companyService;
        private readonly AppointmentDBContext _ctx;
        private readonly string _webRootPath;

        public HomeOfficeController(IClientService clientService, IExpertService expertService, IContactService contactService,
            ICompanyService companyService, AppointmentDBContext ctx, IWebHostEnvironment environment)
        {
            _contactService = contactService;
            _companyService = companyService;
            _clientService = clientService;
            _expertService = expertService;
            _ctx = ctx;
            _webRootPath = environment.WebRootPath;
        }

        // The signed-in office (from the auth cookie claim). Every query below is scoped to it.
        private int CompanyId => User.GetCompanyId().GetValueOrDefault();

        private IQueryable<Appointment> OfficeAppointments => _ctx.Appointment.Where(x => x.Expert.CompanyID == CompanyId);

        private static IQueryable<AgendaItemVM> ToAgenda(IQueryable<Appointment> query) => query.Select(x => new AgendaItemVM
        {
            AppointmentID = x.AppointmentID,
            Subject = x.Subject,
            Start = x.StartTime,
            End = x.EndTime,
            IsFullDay = x.IsFullDay == true,
            ExpertName = x.Expert.Name + " " + x.Expert.Surname,
            ClientName = x.Client != null ? x.Client.Name + " " + x.Client.Surname : null,
            Color = x.ThemeColor
        });

        #region Dashboard

        public IActionResult Index()
        {
            var today = DateTime.Today;
            var tomorrow = today.AddDays(1);
            var weekStart = today.AddDays(-(((int)today.DayOfWeek + 6) % 7)); // Monday
            var weekEnd = weekStart.AddDays(7);

            ClientExpertVM model = new ClientExpertVM();
            model.Company = _companyService.GetCompany(CompanyId);
            model.ClientTotal = _ctx.Client.Count(x => x.CompanyID == CompanyId && x.Continuity);
            model.ExpertsTotal = _ctx.Expert.Count(x => x.CompanyID == CompanyId);
            model.AppsToday = OfficeAppointments.Count(x => x.StartTime >= today && x.StartTime < tomorrow);
            model.AppsThisWeek = OfficeAppointments.Count(x => x.StartTime >= weekStart && x.StartTime < weekEnd);
            model.TodaysAppointments = ToAgenda(OfficeAppointments.Where(x => x.StartTime >= today && x.StartTime < tomorrow).OrderBy(x => x.StartTime)).ToList();
            model.UpcomingAppointments = ToAgenda(OfficeAppointments.Where(x => x.StartTime >= tomorrow).OrderBy(x => x.StartTime).Take(6)).ToList();

            // Age groups of active clients, with real ages (birthday-aware) and non-overlapping ranges.
            var birthDates = _ctx.Client.Where(x => x.CompanyID == CompanyId && x.Continuity).Select(x => x.BirthDate).ToList();
            var groups = birthDates.GroupBy(b => AgeHelper.Group(AgeHelper.Age(b, today))).ToDictionary(g => g.Key, g => g.Count());
            model.AgeGroups = AgeHelper.Groups.Select(g =>
            {
                var count = groups.GetValueOrDefault(g.Name);
                return new AgeGroupVM
                {
                    Name = g.Name,
                    Range = g.Range,
                    Count = count,
                    Percent = birthDates.Count == 0 ? 0 : Math.Round(count * 100m / birthDates.Count, 1)
                };
            }).ToList();

            return View(model);
        }

        #endregion

        #region Experts

        public IActionResult ListExpertInfos()
        {
            var now = DateTime.Now;
            var experts = (from c in _ctx.Expert
                           join co in _ctx.Contact on c.ExpertID equals co.ExpertID
                           where co.IsActive == true && c.CompanyID == CompanyId
                           select new ExpertInfosDTO
                           {
                               ID = c.ExpertID,
                               Uzmanlik = c.Profession,
                               AdSoyad = c.Name + " " + c.Surname,
                               PhotoURL = c.PhotoURL,
                               Telno = co.PhoneNumber1,
                               Email = co.Email,
                               Not = c.Note,
                               UpcomingCount = c.Appointment.Count(a => a.StartTime >= now)
                           }).OrderBy(x => x.AdSoyad).ToList();
            experts.ForEach(x => x.Telno = x.Telno?.Trim());
            return View(experts);
        }

        [HttpGet]
        public IActionResult ExpertDetail(short id)
        {
            var expert = _expertService.FindExpert(x => x.ExpertID == id && x.CompanyID == CompanyId);
            if (expert == null)
            {
                return NotFound();
            }
            var model = new ExpertDetailVM
            {
                Expert = expert,
                Contact = _contactService.Find(x => x.ExpertID == id && x.IsActive == true),
                TotalAppointments = _ctx.Appointment.Count(x => x.ExpertID == id),
                UpcomingAppointments = ToAgenda(_ctx.Appointment.Where(x => x.ExpertID == id && x.StartTime >= DateTime.Today).OrderBy(x => x.StartTime).Take(10)).ToList()
            };
            return View(model);
        }

        [HttpGet]
        public IActionResult ExpertAdd()
        {
            return View("ExpertUpdate", new ExpertVM());
        }

        [HttpGet]
        public IActionResult ExpertUpdate(short id)
        {
            var expert = _expertService.FindExpert(x => x.ExpertID == id && x.CompanyID == CompanyId);
            if (expert == null)
            {
                return NotFound();
            }
            return View(new ExpertVM { Expert = expert, Contact = _contactService.Find(x => x.ExpertID == id && x.IsActive == true) ?? new Contact() });
        }

        [HttpPost]
        public IActionResult ExpertUpdate(ExpertVM model, IEnumerable<IFormFile> file)
        {
            var isNew = model.Expert.ExpertID == 0;
            Expert existing = null;
            if (!isNew)
            {
                existing = _ctx.Expert.AsNoTracking().FirstOrDefault(x => x.ExpertID == model.Expert.ExpertID && x.CompanyID == CompanyId);
                if (existing == null)
                {
                    return NotFound();
                }
            }
            ValidateBirthDate("Expert.BirthDate", model.Expert.BirthDate);
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            // The photo is optional; a new upload replaces the current one.
            model.Expert.PhotoURL = existing?.PhotoURL;
            if (file != null && file.Any())
            {
                var upload = FileDocumentUpload(file, MaxFileSize, ExpertImagesPath, _webRootPath, ImageTypes);
                var failed = upload.FirstOrDefault(x => x.Value != FileResultType.Complete);
                if (failed.Key != null)
                {
                    RemoveAll(upload.Keys, ExpertImagesPath, _webRootPath);
                    ModelState.AddModelError("file", ControlMessages(failed.Value, MaxFileSize).Keys.First());
                    return View(model);
                }
                RemoveAll(new[] { new FileResultItem { UploadPath = existing?.PhotoURL } }, ExpertImagesPath, _webRootPath);
                model.Expert.PhotoURL = upload.Keys.First().UploadPath;
            }

            model.Expert.CompanyID = CompanyId;
            if (isNew)
            {
                _expertService.AddExpert(model.Expert);
            }
            else
            {
                _expertService.UpdateExpert(model.Expert);
            }
            ReplaceContact(model.Contact, expertId: model.Expert.ExpertID);

            TempData["Success"] = isNew ? "Uzman eklendi." : "Uzman bilgileri güncellendi.";
            return RedirectToAction("ExpertDetail", new { id = model.Expert.ExpertID });
        }

        [HttpPost]
        public IActionResult ExpertDelete(short id)
        {
            var expert = _expertService.FindExpert(x => x.ExpertID == id && x.CompanyID == CompanyId);
            if (expert == null)
            {
                return NotFound();
            }
            if (_ctx.Appointment.Any(x => x.ExpertID == id))
            {
                TempData["Error"] = $"{expert.Name} {expert.Surname} adına kayıtlı randevular olduğu için silinemez. Önce randevuları silin veya başka bir uzmana aktarın.";
                return RedirectToAction("ExpertDetail", new { id });
            }
            _expertService.DeleteExpert(id);
            RemoveAll(new[] { new FileResultItem { UploadPath = expert.PhotoURL } }, ExpertImagesPath, _webRootPath);
            TempData["Success"] = "Uzman silindi.";
            return RedirectToAction("ListExpertInfos");
        }

        #endregion

        #region Clients

        [HttpGet]
        public IActionResult ListClientInfos()
        {
            var today = DateTime.Today;
            var clients = (from c in _ctx.Client
                           join co in _ctx.Contact on c.ClientID equals co.ClientID
                           where co.IsActive == true && c.CompanyID == CompanyId
                           select new { c.ClientID, c.TcKimlik, c.Name, c.Surname, c.BirthDate, c.Continuity, co.PhoneNumber1, co.Email, co.Address })
                           .ToList()
                           .Select(x =>
                           {
                               var age = AgeHelper.Age(x.BirthDate, today);
                               return new ClientInfosDTO
                               {
                                   ID = x.ClientID,
                                   Yas = age,
                                   DanisanTuru = AgeHelper.Group(age),
                                   TCKimlik = x.TcKimlik,
                                   AdSoyad = x.Name + " " + x.Surname,
                                   Telno = x.PhoneNumber1?.Trim(),
                                   Email = x.Email,
                                   Adres = x.Address,
                                   DevamDurumu = x.Continuity,
                               };
                           })
                           .OrderByDescending(x => x.DevamDurumu).ThenBy(x => x.AdSoyad)
                           .ToList();
            return View(clients);
        }

        [HttpGet]
        public IActionResult ClientDetail(int id)
        {
            var client = _clientService.FindClient(x => x.ClientID == id && x.CompanyID == CompanyId);
            if (client == null)
            {
                return NotFound();
            }
            var model = new ClientDetailVM
            {
                Client = client,
                Contact = _contactService.Find(x => x.ClientID == id && x.IsActive == true),
                Age = AgeHelper.Age(client.BirthDate, DateTime.Today),
                Appointments = ToAgenda(_ctx.Appointment.Where(x => x.ClientID == id).OrderByDescending(x => x.StartTime).Take(20)).ToList()
            };
            return View(model);
        }

        [HttpGet]
        public IActionResult ClientAdd()
        {
            return View("ClientUpdate", new ClientVM());
        }

        [HttpGet]
        public IActionResult ClientUpdate(int id)
        {
            var client = _clientService.FindClient(x => x.ClientID == id && x.CompanyID == CompanyId);
            if (client == null)
            {
                return NotFound();
            }
            return View(new ClientVM { Client = client, Contact = _contactService.Find(x => x.ClientID == id && x.IsActive == true) ?? new Contact() });
        }

        [HttpPost]
        public IActionResult ClientUpdate(ClientVM model)
        {
            var isNew = model.Client.ClientID == 0;
            if (!isNew && !_ctx.Client.Any(x => x.ClientID == model.Client.ClientID && x.CompanyID == CompanyId))
            {
                return NotFound();
            }
            ValidateBirthDate("Client.BirthDate", model.Client.BirthDate);
            if (!string.IsNullOrEmpty(model.Client.TcKimlik)
                && _ctx.Client.Any(x => x.CompanyID == CompanyId && x.TcKimlik == model.Client.TcKimlik && x.ClientID != model.Client.ClientID))
            {
                ModelState.AddModelError("Client.TcKimlik", "Bu TC Kimlik No ile kayıtlı bir danışan zaten var.");
            }
            // Nothing is changed unless the whole form is valid (the old code deactivated the contact first).
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            model.Client.CompanyID = CompanyId;
            if (isNew)
            {
                model.Client.Continuity = true;
                _clientService.AddClient(model.Client);
            }
            else
            {
                _clientService.UpdateClient(model.Client);
            }
            ReplaceContact(model.Contact, clientId: model.Client.ClientID);

            TempData["Success"] = isNew ? "Danışan eklendi." : "Danışan bilgileri güncellendi.";
            return RedirectToAction("ClientDetail", new { id = model.Client.ClientID });
        }

        [HttpPost]
        public IActionResult ClientDelete(int id)
        {
            var client = _clientService.FindClient(x => x.ClientID == id && x.CompanyID == CompanyId);
            if (client == null)
            {
                return NotFound();
            }
            // Appointments and contact records are removed by the database (cascade delete).
            _clientService.DeleteClient(id);
            TempData["Success"] = $"{client.Name} {client.Surname} ve randevuları silindi.";
            return RedirectToAction("ListClientInfos");
        }

        // The form starts with an empty date (no guessed default), so a missing value must be caught here.
        private void ValidateBirthDate(string key, DateTime value)
        {
            if (ModelState.TryGetValue(key, out var entry) && entry.Errors.Count > 0)
            {
                return;
            }
            if (value == default)
            {
                ModelState.AddModelError(key, "Doğum Tarihi alanı zorunludur.");
            }
            else if (value > DateTime.Today)
            {
                ModelState.AddModelError(key, "Doğum tarihi gelecekte olamaz.");
            }
            else if (value.Year < 1900)
            {
                ModelState.AddModelError(key, "Geçerli bir doğum tarihi giriniz.");
            }
        }

        // Contacts are versioned: the current one is deactivated and the edited copy is added as the active one.
        private void ReplaceContact(Contact input, int? clientId = null, short? expertId = null)
        {
            var current = _contactService.Find(x => x.IsActive == true
                && (clientId != null ? x.ClientID == clientId : x.ExpertID == expertId));
            if (current != null)
            {
                if (current.Email == input.Email && current.PhoneNumber1?.Trim() == input.PhoneNumber1?.Trim()
                    && current.PhoneNumber2?.Trim() == input.PhoneNumber2?.Trim() && current.Address == input.Address)
                {
                    return;
                }
                current.IsActive = false;
                _contactService.UpdateContact(current);
            }
            _contactService.AddContact(new Contact
            {
                ClientID = clientId,
                ExpertID = expertId,
                Address = input.Address,
                PhoneNumber1 = input.PhoneNumber1,
                PhoneNumber2 = input.PhoneNumber2,
                Email = input.Email,
                IsActive = true
            });
        }

        #endregion

        #region TAKVIM

        [HttpGet]
        public IActionResult AppointmentOpss(short? expertId)
        {
            DDLsVM model = new DDLsVM();
            model.SelectedExpertId = expertId ?? 0;
            model.ExpertsList = _ctx.Expert.Where(x => x.CompanyID == CompanyId).OrderBy(a => a.Name)
                .Select(a => new SelectListItem { Text = a.Name + " " + a.Surname, Value = a.ExpertID.ToString() }).ToList();
            model.ClientsList = _ctx.Client.Where(x => x.Continuity == true && x.CompanyID == CompanyId).OrderBy(a => a.Name)
                .Select(a => new SelectListItem { Text = a.Name + " " + a.Surname, Value = a.ClientID.ToString() }).ToList();
            return View(model);
        }

        // FullCalendar asks for the visible range only. It sends "2026-10-12T00:00:00+03:00"; appointments are stored
        // as wall-clock times, so the offset is dropped instead of converting to UTC (the default DateTime binding).
        [HttpGet]
        public JsonResult GetEvents(string start, string end, short? expertId)
        {
            var query = OfficeAppointments;
            if (DateTimeOffset.TryParse(start, out var rangeStart))
            {
                var from = rangeStart.DateTime;
                query = query.Where(x => (x.EndTime ?? x.StartTime) >= from);
            }
            if (DateTimeOffset.TryParse(end, out var rangeEnd))
            {
                var to = rangeEnd.DateTime;
                query = query.Where(x => x.StartTime < to);
            }
            if (expertId != null)
            {
                query = query.Where(x => x.ExpertID == expertId);
            }
            var events = query
                .Select(x => new
                {
                    x.AppointmentID,
                    x.ExpertID,
                    x.ClientID,
                    x.Subject,
                    x.Description,
                    x.StartTime,
                    x.EndTime,
                    x.ThemeColor,
                    IsFullDay = x.IsFullDay == true,
                    ExpertName = x.Expert.Name + " " + x.Expert.Surname,
                    ClientName = x.Client != null ? x.Client.Name + " " + x.Client.Surname : null
                })
                .ToList();
            return Json(events);
        }

        [HttpPost]
        public JsonResult SaveEvent(AppointmentInput data)
        {
            string error = null;
            if (string.IsNullOrWhiteSpace(data.Subject))
            {
                error = "Konu boş olamaz.";
            }
            else if (data.ExpertID == null || !_ctx.Expert.Any(x => x.ExpertID == data.ExpertID && x.CompanyID == CompanyId))
            {
                error = "Geçerli bir uzman seçiniz.";
            }
            else if (data.ClientID == null || !_ctx.Client.Any(x => x.ClientID == data.ClientID && x.CompanyID == CompanyId))
            {
                error = "Geçerli bir danışan seçiniz.";
            }
            else if (!data.IsFullDay && (data.EndTime == null || data.EndTime <= data.StartTime))
            {
                error = "Bitiş zamanı başlangıçtan sonra olmalıdır.";
            }
            if (error != null)
            {
                return Json(new { status = false, message = error });
            }

            var end = data.IsFullDay ? (DateTime?)null : data.EndTime;
            if (!data.IsFullDay)
            {
                // An expert cannot be in two timed appointments at once.
                var clash = _ctx.Appointment.Where(x => x.ExpertID == data.ExpertID && x.AppointmentID != data.AppointmentID
                        && x.IsFullDay != true && x.StartTime < end && x.EndTime > data.StartTime)
                    .Select(x => new { x.Subject, x.StartTime, x.EndTime }).FirstOrDefault();
                if (clash != null)
                {
                    return Json(new { status = false, message = $"Uzmanın {clash.StartTime:HH:mm}–{clash.EndTime:HH:mm} arasında \"{clash.Subject}\" randevusu var." });
                }
            }

            Appointment appointment;
            if (data.AppointmentID > 0)
            {
                appointment = OfficeAppointments.FirstOrDefault(x => x.AppointmentID == data.AppointmentID);
                if (appointment == null)
                {
                    return Json(new { status = false, message = "Randevu bulunamadı." });
                }
            }
            else
            {
                appointment = new Appointment();
                _ctx.Appointment.Add(appointment);
            }
            appointment.ExpertID = data.ExpertID;
            appointment.ClientID = data.ClientID;
            appointment.Subject = data.Subject.Trim();
            appointment.Description = data.Description ?? "";
            appointment.ThemeColor = string.IsNullOrWhiteSpace(data.ThemeColor) ? AppointmentColors.Default : data.ThemeColor;
            appointment.IsFullDay = data.IsFullDay;
            appointment.StartTime = data.IsFullDay ? data.StartTime.Date : data.StartTime;
            appointment.EndTime = end;
            _ctx.SaveChanges();

            return Json(new { status = true, id = appointment.AppointmentID });
        }

        [HttpPost]
        public JsonResult DeleteEvent(int id)
        {
            var status = false;

            Appointment deletedEvent = OfficeAppointments.FirstOrDefault(x => x.AppointmentID == id);
            if (deletedEvent != null)
            {
                _ctx.Appointment.Remove(deletedEvent);
                _ctx.SaveChanges();
                status = true;
            }
            return Json(new { status = status });
        }
        #endregion
    }
}
