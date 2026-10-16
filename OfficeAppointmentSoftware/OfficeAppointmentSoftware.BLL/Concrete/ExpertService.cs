using OfficeAppointmentSoftware.BLL.Abstracts;
using OfficeAppointmentSoftware.DAL.Repositories.Abstracts;
using OfficeAppointmentSoftware.Entities.Entities;
using System.Linq.Expressions;

namespace OfficeAppointmentSoftware.BLL.Concrete
{
    public class ExpertService : IExpertService
    {
        private readonly IExpertRepository _expertRepository;
        public ExpertService(IExpertRepository expertRepository)
        {
            _expertRepository = expertRepository;
        }
        public void AddExpert(Expert expert)
        {
            _expertRepository.Add(expert);
        }

        public void DeleteExpert(short id)
        {
            _expertRepository.Delete(id);
        }

        public Expert FindByIdExpert(short id)
        {
          return  _expertRepository.FindById(id);
        }

        public Expert FindExpert(Expression<Func<Expert, bool>> lambda)
        {
            return _expertRepository.Find(lambda);
        }

        public IEnumerable<Expert> SelectExpert(Expression<Func<Expert, bool>> lambda = null)
        {
           return _expertRepository.Select(lambda);
        }

        public void UpdateExpert(Expert expert)
        {
            _expertRepository.Update(expert);
        }
    }
}
