using OfficeAppointmentSoftware.BLL.Abstracts.AdminServices;
using OfficeAppointmentSoftware.DAL.Repositories.Abstracts.AdminRepositories;
using OfficeAppointmentSoftware.Entities.Entities;
using System.Linq.Expressions;

namespace OfficeAppointmentSoftware.BLL.Concrete.AdminServices
{
    public class WorksService : IWorksService
    {
        private readonly IWorksRepository _worksRepository;
        public WorksService(IWorksRepository worksRepository)
        {
            _worksRepository = worksRepository;
        }
        public void AddWork(Works work)
        {
            _worksRepository.Add(work);
        }

        public void DeleteWork(int id)
        {
            _worksRepository.Delete(id);
        }

        public IEnumerable<Works> GetWorks(Expression<Func<Works, bool>> lambda = null)
        {
            return _worksRepository.Select(lambda);
        }

        public void Update(Works work)
        {
            _worksRepository.Update(work);
        }
    }
}
