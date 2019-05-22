using OfficeAppointmentSoftware.Entities.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace OfficeAppointmentSoftware.BLL.Abstracts
{
    public interface IExpertService
    {
        void AddExpert(Expert expert);
        void DeleteExpert(short id);
        void UpdateExpert(Expert expert);
        Expert FindByIdExpert(short id);
        Expert FindExpert(Expression<Func<Expert, bool>> lambda);
        IEnumerable<Expert> SelectExpert(System.Linq.Expressions.Expression<Func<Expert, bool>> lambda = null);
    }
}
