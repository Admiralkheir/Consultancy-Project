using OfficeAppointmentSoftware.Entities.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OfficeAppointmentSoftware.BLL.Abstracts.AdminServices
{
    public interface IFeaturesService 
    {
        void AddFeatures(Features feature);
        void DeleteFeatures(short id);
        IEnumerable<Features> GetFeatures(System.Linq.Expressions.Expression<Func<Features, bool>> lambda = null);
        void UpdateFeatures(Features feature);
    }
}
