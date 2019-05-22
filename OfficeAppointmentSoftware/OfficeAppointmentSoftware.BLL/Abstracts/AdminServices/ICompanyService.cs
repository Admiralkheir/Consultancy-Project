using OfficeAppointmentSoftware.Entities.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OfficeAppointmentSoftware.BLL.Abstracts.AdminServices
{
    public interface ICompanyService
    {
        void AddCompany(Company company);
        void UpdateCompany(Company company);
        void DeleteCompany(int id);
    }
}
