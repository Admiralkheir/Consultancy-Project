using Microsoft.EntityFrameworkCore;
using OfficeAppointmentSoftware.DAL.Contexts;
using OfficeAppointmentSoftware.DAL.Repositories.Abstracts.Core;

namespace OfficeAppointmentSoftware.DAL.Repositories.Concretes.Core
{
    public class BaseRepository<TEntity, TKey> : IBaseRepository<TEntity, TKey> where TEntity : class
    {
        // The context is owned by the DI scope (one per request), so the repository never disposes it.
        protected readonly AppointmentDBContext _context;
        protected readonly DbSet<TEntity> _dbSet;

        public BaseRepository(AppointmentDBContext context)
        {
            _context = context;
            _dbSet = _context.Set<TEntity>();
        }

        public void Add(TEntity entity)
        {
            var entry = _context.Entry(entity);
            entry.State = EntityState.Added;
            _context.SaveChanges();
        }

        public void Delete(TKey key)
        {
            var entity = _dbSet.Find(key);
            var entry = _context.Entry(entity);
            entry.State = EntityState.Deleted;
            _context.SaveChanges();
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
        }
    }
}
