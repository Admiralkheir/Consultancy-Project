using OfficeAppointmentSoftware.BLL.Abstracts.AdminServices;
using OfficeAppointmentSoftware.DAL.Repositories.Concretes.AdminRepositories;
using OfficeAppointmentSoftware.Entities.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OfficeAppointmentSoftware.BLL.Concrete.AdminServices
{
    public class CompanyService : CompanyRepository , ICompanyService
    {
        CompanyRepository _companyRepository;
        public CompanyService()
        {
            _companyRepository = new CompanyRepository();
        }
        public void AddCompany(Company company)
        {
           _companyRepository.Add(company);
        }

        public void DeleteCompany(int id)
        {
            _companyRepository.Delete(id);
        }

        public void UpdateCompany(Company company)
        {
            _companyRepository.Update(company);
        }
    }
}
