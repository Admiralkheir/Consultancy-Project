using OfficeAppointmentSoftware.Entities.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace OfficeAppointmentSoftware.BLL.Abstracts
{
    public interface IClientService
    {
        void AddClient(Client client);
        void DeleteClient(int id);
        void UpdateClient(Client client);
        Client FindByIdClient(int id);
        Client FindClient(Expression<Func<Client, bool>> lambda);
        IEnumerable<Client> SelectClient(System.Linq.Expressions.Expression<Func<Client, bool>> lambda = null);
    }
}
