using OfficeAppointmentSoftware.BLL.Abstracts.AdminServices;
using OfficeAppointmentSoftware.DAL.Repositories.Concretes.AdminRepositories;
using OfficeAppointmentSoftware.Entities.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace OfficeAppointmentSoftware.BLL.Concrete.AdminServices
{
    public class PlanService : IPlanService
    {
        PlanRepository _planRepository;
        public PlanService()
        {
            _planRepository = new PlanRepository();
        }
        public IEnumerable<Plans> GetPlans(Expression<Func<Features, bool>> lambda = null)
        {
            return _planRepository.Select();
        }
    }
}
