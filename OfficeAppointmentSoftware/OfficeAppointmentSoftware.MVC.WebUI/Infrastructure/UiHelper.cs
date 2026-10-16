using System.Globalization;

namespace OfficeAppointmentSoftware.MVC.WebUI.Infrastructure
{
    public static class UiHelper
    {
        private static readonly CultureInfo Turkish = new("tr-TR");
        private static readonly string[] AvatarColors = { "#6366f1", "#0d9488", "#d97706", "#db2777", "#7c3aed", "#2563eb", "#059669", "#dc2626" };

        public static string Initials(string fullName)
        {
            var parts = (fullName ?? "").Split(' ', StringSplitOptions.RemoveEmptyEntries);
            var letters = parts.Length switch
            {
                0 => "?",
                1 => parts[0][..1],
                _ => parts[0][..1] + parts[^1][..1]
            };
            return letters.ToUpper(Turkish);
        }

        // Stable colour per name, so a person keeps the same avatar colour everywhere.
        public static string AvatarColor(string name)
        {
            var hash = 0;
            foreach (var c in name ?? "")
            {
                hash = unchecked(hash * 31 + c);
            }
            return AvatarColors[Math.Abs(hash % AvatarColors.Length)];
        }

        public static string Date(DateTime value) => value.ToString("d MMMM yyyy", Turkish);
        public static string DayName(DateTime value) => value.ToString("dddd", Turkish);
        public static string ShortDate(DateTime value) => value.ToString("d MMM", Turkish);
        public static string Time(DateTime value) => value.ToString("HH:mm", Turkish);
        public static string Money(decimal value) => value.ToString("N0", Turkish) + " ₺";
    }
}
