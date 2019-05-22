using OfficeAppointmentSoftware.Entities.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace OfficeAppointmentSoftware.BLL.Abstracts
{
    public interface IContactService 
    {
        void AddContact(Contact contact);
        void DeleteContact(int id);
        void UpdateContact(Contact expert);
        Contact FindByIdContact(int id);
        Contact Find(Expression<Func<Contact, bool>> lambda);
    }
}
