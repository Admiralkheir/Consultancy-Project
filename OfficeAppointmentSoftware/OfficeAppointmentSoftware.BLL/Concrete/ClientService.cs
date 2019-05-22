using OfficeAppointmentSoftware.BLL.Abstracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OfficeAppointmentSoftware.Entities.Entities;
using System.Linq.Expressions;
using OfficeAppointmentSoftware.DAL.Repositories.Concretes;

namespace OfficeAppointmentSoftware.BLL.Concrete
{
    public class ClientService : ClientRepository, IClientService
    {
        ClientRepository _clientRepository;
        public ClientService()
        {
            _clientRepository = new ClientRepository();

        }
        public void AddClient(Client client)
        {
            _clientRepository.Add(client);
        }

        public void DeleteClient(int id)
        {
            _clientRepository.Delete(id);
        }

        public Client FindByIdClient(int id)
        {
            return _clientRepository.FindById(id);
        }

        public Client FindClient(Expression<Func<Client, bool>> lambda)
        {
            return _clientRepository.Find(lambda);
        }

        public IEnumerable<Client> SelectClient(Expression<Func<Client, bool>> lambda = null)
        {
            return _clientRepository.Select(lambda);
        }

        public void UpdateClient(Client client)
        {
            _clientRepository.Update(client);
        }
    }
}
