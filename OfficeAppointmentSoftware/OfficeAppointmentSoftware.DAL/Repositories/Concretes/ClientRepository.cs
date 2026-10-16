using OfficeAppointmentSoftware.DAL.Contexts;
using OfficeAppointmentSoftware.DAL.Repositories.Abstracts;
using OfficeAppointmentSoftware.DAL.Repositories.Concretes.Core;
using OfficeAppointmentSoftware.Entities.Entities;

namespace OfficeAppointmentSoftware.DAL.Repositories.Concretes
{
    public class ClientRepository : BaseRepository<Client, int>, IClientRepository
    {
        public ClientRepository(AppointmentDBContext context) : base(context)
        {
        }
    }
}
