using OfficeAppointmentSoftware.BLL.Abstracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OfficeAppointmentSoftware.Entities.Entities;
using System.Linq.Expressions;
using OfficeAppointmentSoftware.DAL.Repositories.Concretes;

namespace OfficeAppointmentSoftware.BLL.Concrete
{
    public class ExpertService : ExpertRepository,IExpertService
    {
        ExpertRepository _expertRepository;
        public ExpertService()
        {
            _expertRepository = new ExpertRepository();
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
