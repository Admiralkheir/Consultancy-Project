using OfficeAppointmentSoftware.DAL.Contexts;
using OfficeAppointmentSoftware.DAL.Repositories.Abstracts;
using OfficeAppointmentSoftware.DAL.Repositories.Concretes.Core;
using OfficeAppointmentSoftware.Entities.Entities;

namespace OfficeAppointmentSoftware.DAL.Repositories.Concretes
{
    public class ContactRepository : BaseRepository<Contact, int>, IContactRepository
    {
        public ContactRepository(AppointmentDBContext context) : base(context)
        {
        }
    }
}
