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
    public class FeaturesService : FeaturesRepository, IFeaturesService
    {
        FeaturesRepository _featuresRepository;
        public FeaturesService()
        {
            _featuresRepository = new FeaturesRepository();
        }
        public void AddFeatures(Features feature)
        {
            _featuresRepository.Add(feature);
        }

        public void DeleteFeatures(short id)
        {
            _featuresRepository.Delete(id);
        }

        public IEnumerable<Features> GetFeatures(Expression<Func<Features, bool>> lambda = null)
        {
            return _featuresRepository.Select(lambda);
        }

        public void UpdateFeatures(Features feature)
        {
            _featuresRepository.Update(feature);
        }
    }
}
