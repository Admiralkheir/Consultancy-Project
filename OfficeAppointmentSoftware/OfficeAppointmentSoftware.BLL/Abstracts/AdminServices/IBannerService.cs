using OfficeAppointmentSoftware.Entities.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OfficeAppointmentSoftware.BLL.Abstracts.AdminServices
{
  public interface IBannerService
    {
        void BannerAdd(Banner banner) ;
        void BannerUpdate(Banner banner);
        Banner BannerGet(short id);
    }
}
