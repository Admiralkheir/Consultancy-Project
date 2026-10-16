using OfficeAppointmentSoftware.Entities.Entities;

namespace OfficeAppointmentSoftware.BLL.Abstracts.AdminServices
{
    public interface ICompanyService
    {
        void AddCompany(Company company);
        void UpdateCompany(Company company);
        void DeleteCompany(int id);
        Company GetCompany(int id);
    }
}
