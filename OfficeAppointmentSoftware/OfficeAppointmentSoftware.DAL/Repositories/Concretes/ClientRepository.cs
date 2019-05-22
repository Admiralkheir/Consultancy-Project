using OfficeAppointmentSoftware.DAL.Repositories.Abstracts;
using OfficeAppointmentSoftware.DAL.Repositories.Concretes.Core;
using OfficeAppointmentSoftware.Entities.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OfficeAppointmentSoftware.DAL.Repositories.Concretes
{
   public class ClientRepository : BaseRepository<Client,int> , IClientRepository
    {
        
    }
}
