using OfficeAppointmentSoftware.BLL.Abstracts;
using OfficeAppointmentSoftware.DAL.Repositories.Abstracts;
using OfficeAppointmentSoftware.Entities.Entities;
using System.Linq.Expressions;

namespace OfficeAppointmentSoftware.BLL.Concrete
{
    public class ClientService : IClientService
    {
        private readonly IClientRepository _clientRepository;
        public ClientService(IClientRepository clientRepository)
        {
            _clientRepository = clientRepository;
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
