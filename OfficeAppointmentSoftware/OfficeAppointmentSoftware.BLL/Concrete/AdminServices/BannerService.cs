using OfficeAppointmentSoftware.BLL.Abstracts.AdminServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OfficeAppointmentSoftware.Entities.Entities;
using OfficeAppointmentSoftware.DAL.Repositories.Concretes.AdminRepositories;

namespace OfficeAppointmentSoftware.BLL.Concrete.AdminServices
{
    public class BannerService : BannerRepository, IBannerService
    {
        BannerRepository _bannerRepository;
        public BannerService()
        {
            _bannerRepository = new BannerRepository();
        }
        public void BannerAdd(Banner banner)
        {
            _bannerRepository.Add(banner);
        }

        public Banner BannerGet(short id)
        {
            return _bannerRepository.FindById(id);
        }

        public void BannerUpdate(Banner banner)
        {
            _bannerRepository.Update(banner);
        }
    }
}
