using System.Web.Mvc;

namespace OfficeAppointmentSoftware.MVC.WebUI.Areas.Ofiice
{
    public class OfiiceAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "Ofiice";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "Ofiice_default",
                "Ofiice/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}