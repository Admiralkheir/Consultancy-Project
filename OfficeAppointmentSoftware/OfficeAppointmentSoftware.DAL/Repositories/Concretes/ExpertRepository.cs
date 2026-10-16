using OfficeAppointmentSoftware.DAL.Contexts;
using OfficeAppointmentSoftware.DAL.Repositories.Abstracts;
using OfficeAppointmentSoftware.DAL.Repositories.Concretes.Core;
using OfficeAppointmentSoftware.Entities.Entities;

namespace OfficeAppointmentSoftware.DAL.Repositories.Concretes
{
    public class ExpertRepository : BaseRepository<Expert, short>, IExpertRepository
    {
        public ExpertRepository(AppointmentDBContext context) : base(context)
        {
        }
    }
}
