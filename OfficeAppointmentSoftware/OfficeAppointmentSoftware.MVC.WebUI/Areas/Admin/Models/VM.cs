namespace OfficeAppointmentSoftware.MVC.WebUI.Areas.Admin.Models
{
    // One row of the admin's office list.
    public class OfficeListItemVM
    {
        public int CompanyID { get; set; }
        public string CompanyName { get; set; }
        public string OwnerName { get; set; }
        public string LogoURL { get; set; }
        public string PlanName { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public int ExpertCount { get; set; }
        public int ClientCount { get; set; }
    }

    public class PlanListItemVM
    {
        public short PlanID { get; set; }
        public string PlanName { get; set; }
        public decimal Price { get; set; }
        public int OfficeCount { get; set; }
    }
}
