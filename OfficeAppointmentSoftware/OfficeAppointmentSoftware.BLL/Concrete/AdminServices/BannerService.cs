using OfficeAppointmentSoftware.BLL.Abstracts.AdminServices;
using OfficeAppointmentSoftware.DAL.Repositories.Abstracts.AdminRepositories;
using OfficeAppointmentSoftware.Entities.Entities;

namespace OfficeAppointmentSoftware.BLL.Concrete.AdminServices
{
    public class BannerService : IBannerService
    {
        private readonly IBannerRepository _bannerRepository;
        public BannerService(IBannerRepository bannerRepository)
        {
            _bannerRepository = bannerRepository;
        }
        public void BannerAdd(Banner banner)
        {
            _bannerRepository.Add(banner);
        }

        public Banner BannerGet(short id)
        {
            return _bannerRepository.FindById(id);
        }

        public Banner BannerGetDefault()
        {
            return _bannerRepository.Select().OrderBy(x => x.BannerID).FirstOrDefault();
        }

        public void BannerUpdate(Banner banner)
        {
            _bannerRepository.Update(banner);
        }
    }
}
