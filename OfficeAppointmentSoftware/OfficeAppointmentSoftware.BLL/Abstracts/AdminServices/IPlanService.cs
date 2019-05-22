using OfficeAppointmentSoftware.Entities.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OfficeAppointmentSoftware.BLL.Abstracts.AdminServices
{
    public interface IPlanService
    {
        IEnumerable<Plans> GetPlans(System.Linq.Expressions.Expression<Func<Features, bool>> lambda = null);
    }
}
