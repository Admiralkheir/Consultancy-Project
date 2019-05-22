using OfficeAppointmentSoftware.BLL.Abstracts.AdminServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OfficeAppointmentSoftware.Entities.Entities;
using OfficeAppointmentSoftware.DAL.Repositories.Concretes.AdminRepositories;
using System.Linq.Expressions;

namespace OfficeAppointmentSoftware.BLL.Concrete.AdminServices
{
   public class WorksService : WorksRepository, IWorksService
    {
        WorksRepository _worksRepository;
        public  WorksService()
        {
            _worksRepository = new WorksRepository();
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
