using OfficeAppointmentSoftware.Entities.Entities;

namespace OfficeAppointmentSoftware.BLL.Abstracts.AdminServices
{
    public interface IBannerService
    {
        void BannerAdd(Banner banner);
        void BannerUpdate(Banner banner);
        Banner BannerGet(short id);
        // The site shows a single banner: the one with the lowest id, or null when there is none.
        Banner BannerGetDefault();
    }
}
