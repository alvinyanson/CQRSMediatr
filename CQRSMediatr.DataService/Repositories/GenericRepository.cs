
using CQRSMediatr.DataService.Data;
using CQRSMediatr.DataService.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace CQRSMediatr.DataService.Repositories
{
    public abstract class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        protected readonly ILogger _logger;
        private readonly AppDbContext _context;

        internal DbSet<T> _dbSet;

        public GenericRepository(ILogger logger, AppDbContext context)
        {
            _logger = logger;

            _context = context;

            _dbSet = context.Set<T>();
        }


        public virtual async Task<bool> Add(T entity)
        {
            await _dbSet.AddAsync(entity);

            return true;
        }

        public virtual Task<IEnumerable<T>> All()
        {
            throw new NotImplementedException();
        }

        public virtual Task<bool> Delete(Guid id)
        {
            throw new NotImplementedException();
        }

        public virtual async Task<T?> GetById(Guid id)
        {
            return await _dbSet.FindAsync(id);
        }

        public virtual Task<bool> Update(T entity)
        {
            throw new NotImplementedException();
        }
    }
}
