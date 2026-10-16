using OfficeAppointmentSoftware.Entities.Entities;

namespace OfficeAppointmentSoftware.BLL.Abstracts.AdminServices
{
    public interface IPlanService
    {
        IEnumerable<Plans> GetPlans(System.Linq.Expressions.Expression<Func<Features, bool>> lambda = null);
        Plans GetPlan(short id);
        void AddPlan(Plans plan);
        void UpdatePlan(Plans plan);
        void DeletePlan(short id);
    }
}
