using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace OfficeAppointmentSoftware.DAL.Repositories.Abstracts.Core
{
    public interface IBaseRepository <TEntity,TKey>
    {
        void Add(TEntity entity);
        void Update(TEntity entity);
        void Delete(TKey key);

        TEntity FindById(TKey key);
        TEntity Find(Expression<Func<TEntity, bool>> lambda);
        IEnumerable<TEntity> Select(Expression<Func<TEntity, bool>> lambda=null);




    }
}
