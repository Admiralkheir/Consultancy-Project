using OfficeAppointmentSoftware.BLL.Abstracts.AdminServices;
using OfficeAppointmentSoftware.DAL.Repositories.Abstracts.AdminRepositories;
using OfficeAppointmentSoftware.Entities.Entities;

namespace OfficeAppointmentSoftware.BLL.Concrete.AdminServices
{
    public class CompanyService : ICompanyService
    {
        private readonly ICompanyRepository _companyRepository;
        public CompanyService(ICompanyRepository companyRepository)
        {
            _companyRepository = companyRepository;
        }
        public void AddCompany(Company company)
        {
           _companyRepository.Add(company);
        }

        public void DeleteCompany(int id)
        {
            _companyRepository.Delete(id);
        }

        public Company GetCompany(int id)
        {
            return _companyRepository.FindById(id);
        }

        public void UpdateCompany(Company company)
        {
            _companyRepository.Update(company);
        }
    }
}
