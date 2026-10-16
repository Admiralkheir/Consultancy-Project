using OfficeAppointmentSoftware.DAL.Contexts;
using OfficeAppointmentSoftware.DAL.Repositories.Abstracts.AdminRepositories;
using OfficeAppointmentSoftware.DAL.Repositories.Concretes.Core;
using OfficeAppointmentSoftware.Entities.Entities;

namespace OfficeAppointmentSoftware.DAL.Repositories.Concretes.AdminRepositories
{
    public class CompanyRepository : BaseRepository<Company, int>, ICompanyRepository
    {
        public CompanyRepository(AppointmentDBContext context) : base(context)
        {
        }
    }
}
