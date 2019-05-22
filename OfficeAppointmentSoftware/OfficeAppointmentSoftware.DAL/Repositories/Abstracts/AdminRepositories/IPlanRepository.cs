using OfficeAppointmentSoftware.DAL.Repositories.Abstracts.Core;
using OfficeAppointmentSoftware.Entities.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OfficeAppointmentSoftware.DAL.Repositories.Abstracts.AdminRepositories
{
    interface IPlanRepository :IBaseRepository<Plans,short>
    {
    }
}
