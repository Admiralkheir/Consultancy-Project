using OfficeAppointmentSoftware.DAL.Contexts;
using OfficeAppointmentSoftware.DAL.Repositories.Abstracts.Core;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OfficeAppointmentSoftware.DAL.Repositories.Concretes.Core
{
    public class BaseRepository<TEntity, TKey> : IBaseRepository<TEntity, TKey> , IDisposable where TEntity : class
 {
        AppointmentDBContext _context;
        protected IDbSet<TEntity>  _dbSet;
        public BaseRepository()
    {
            _context = new AppointmentDBContext();
            _dbSet = _context.Set<TEntity>(); 

    }
   
        public void Add(TEntity entity)
        {
            var entry = _context.Entry(entity);
            entry.State = EntityState.Added;
            _context.SaveChanges();
            Dispose();
        }

        public void Delete(TKey key)
        {
            var entity = _dbSet.Find(key);
            var entry = _context.Entry(entity);
            entry.State = EntityState.Deleted;
            _context.SaveChanges();
            Dispose();
        }
        
        public void Dispose()
        {
            _context.Dispose();
        }

        public TEntity Find(System.Linq.Expressions.Expression<Func<TEntity, bool>> lambda)
        {
            return _dbSet.Where(lambda).FirstOrDefault();
        }

        public TEntity FindById(TKey key)
        {
            return _dbSet.Find(key);
        }

        public IEnumerable<TEntity> Select(System.Linq.Expressions.Expression<Func<TEntity, bool>> lambda = null)
        {
            if (lambda == null)
            {
                return _dbSet.AsEnumerable();
            }
            else
            {
                return _dbSet.Where(lambda);
            }

        }

        public void Update(TEntity entity)
        {
            var entry = _context.Entry(entity);
            entry.State = EntityState.Modified;
            _context.SaveChanges();
            Dispose();
        }
    }
}
