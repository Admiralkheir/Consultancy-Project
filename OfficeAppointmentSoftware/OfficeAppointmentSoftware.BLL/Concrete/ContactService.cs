using OfficeAppointmentSoftware.BLL.Abstracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OfficeAppointmentSoftware.Entities.Entities;
using OfficeAppointmentSoftware.DAL.Repositories.Concretes;
using System.Linq.Expressions;
using OfficeAppointmentSoftware.DAL.Repositories.Abstracts;

namespace OfficeAppointmentSoftware.BLL.Concrete
{
    public class ContactService : IContactService
    {
        ContactRepository _contactRepository;
        public  ContactService()
        {
            _contactRepository = new ContactRepository();
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
