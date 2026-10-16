using OfficeAppointmentSoftware.BLL.Abstracts.AdminServices;
using OfficeAppointmentSoftware.DAL.Repositories.Abstracts.AdminRepositories;
using OfficeAppointmentSoftware.Entities.Entities;
using System.Linq.Expressions;

namespace OfficeAppointmentSoftware.BLL.Concrete.AdminServices
{
    public class PlanService : IPlanService
    {
        private readonly IPlanRepository _planRepository;
        public PlanService(IPlanRepository planRepository)
        {
            _planRepository = planRepository;
        }
        public IEnumerable<Plans> GetPlans(Expression<Func<Features, bool>> lambda = null)
        {
            return _planRepository.Select();
        }

        public Plans GetPlan(short id)
        {
            return _planRepository.FindById(id);
        }

        public void AddPlan(Plans plan)
        {
            _planRepository.Add(plan);
        }

        public void UpdatePlan(Plans plan)
        {
            _planRepository.Update(plan);
        }

        public void DeletePlan(short id)
        {
            _planRepository.Delete(id);
        }
    }
}
