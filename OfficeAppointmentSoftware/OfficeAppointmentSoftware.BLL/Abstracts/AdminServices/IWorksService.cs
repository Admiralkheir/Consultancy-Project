using OfficeAppointmentSoftware.Entities.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OfficeAppointmentSoftware.BLL.Abstracts.AdminServices
{
    public interface IWorksService
    {
        void AddWork(Works work);
        void DeleteWork(int id);
        void Update(Works work);
        IEnumerable<Works> GetWorks(System.Linq.Expressions.Expression<Func<Works, bool>> lambda = null);
    }
}
