using OfficeAppointmentSoftware.BLL.Abstracts;
using OfficeAppointmentSoftware.DAL.Repositories.Abstracts;
using OfficeAppointmentSoftware.Entities.Entities;
using System.Linq.Expressions;

namespace OfficeAppointmentSoftware.BLL.Concrete
{
    public class ContactService : IContactService
    {
        private readonly IContactRepository _contactRepository;
        public ContactService(IContactRepository contactRepository)
        {
            _contactRepository = contactRepository;
        }
        public void AddContact(Contact contact)
        {
            _contactRepository.Add(contact);
        }

        public void DeleteContact(int id)
        {
            _contactRepository.Delete(id);
        }

        public Contact Find(Expression<Func<Contact, bool>> lambda)
        {
           return _contactRepository.Find(lambda);
        }

        public Contact FindByIdContact(int id)
        {
            return _contactRepository.FindById(id);
        }

        public void UpdateContact(Contact contact)
        {
             _contactRepository.Update(contact);
        }
    }
}
