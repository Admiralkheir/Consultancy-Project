using OfficeAppointmentSoftware.BLL.Abstracts.AdminServices;
using OfficeAppointmentSoftware.DAL.Repositories.Abstracts.AdminRepositories;
using OfficeAppointmentSoftware.Entities.Entities;
using System.Linq.Expressions;

namespace OfficeAppointmentSoftware.BLL.Concrete.AdminServices
{
    public class FeaturesService : IFeaturesService
    {
        private readonly IFeaturesRepository _featuresRepository;
        public FeaturesService(IFeaturesRepository featuresRepository)
        {
            _featuresRepository = featuresRepository;
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
