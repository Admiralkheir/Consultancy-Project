using OfficeAppointmentSoftware.DAL.Contexts;
using OfficeAppointmentSoftware.DAL.Repositories.Abstracts.AdminRepositories;
using OfficeAppointmentSoftware.DAL.Repositories.Concretes.Core;
using OfficeAppointmentSoftware.Entities.Entities;

namespace OfficeAppointmentSoftware.DAL.Repositories.Concretes.AdminRepositories
{
    public class FeaturesRepository : BaseRepository<Features, short>, IFeaturesRepository
    {
        public FeaturesRepository(AppointmentDBContext context) : base(context)
        {
        }
    }
}
