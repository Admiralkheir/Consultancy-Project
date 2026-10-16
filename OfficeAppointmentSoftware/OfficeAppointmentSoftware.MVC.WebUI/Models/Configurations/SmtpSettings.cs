namespace OfficeAppointmentSoftware.MVC.WebUI.Models.Configurations
{
    // Bound from the "Smtp" configuration section. Keep the password in user secrets or
    // environment variables (Smtp__Password), never in appsettings.json.
    public class SmtpSettings
    {
        public string Host { get; set; }
        public int Port { get; set; } = 587;
        public bool EnableSsl { get; set; } = true;
        public string UserName { get; set; }
        public string Password { get; set; }
        public string FromAddress { get; set; }
        public string FromName { get; set; }
        public string ToAddress { get; set; }
        public string ToName { get; set; }
    }
}
