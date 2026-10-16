namespace OfficeAppointmentSoftware.MVC.WebUI.Infrastructure
{
    public static class AgeHelper
    {
        // Full years, taking the birthday within the year into account.
        public static int Age(DateTime birthDate, DateTime today)
        {
            var age = today.Year - birthDate.Year;
            if (birthDate.Date > today.AddYears(-age))
            {
                age--;
            }
            return Math.Max(age, 0);
        }

        // Client groups used on the dashboard and in the client list. The ranges do not overlap.
        public static string Group(int age) => age switch
        {
            < 12 => "Çocuk",
            < 18 => "Ergen",
            < 40 => "Yetişkin",
            _ => "40 yaş üstü"
        };

        public static readonly (string Name, string Range)[] Groups =
        {
            ("Çocuk", "0–11 yaş"),
            ("Ergen", "12–17 yaş"),
            ("Yetişkin", "18–39 yaş"),
            ("40 yaş üstü", "40 yaş ve üzeri")
        };
    }
}
