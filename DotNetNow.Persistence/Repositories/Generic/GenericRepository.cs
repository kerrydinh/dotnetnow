using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using DotNetNow.Domain;
using DotNetNow.Persistence.Interface;

namespace DotNetNow.Persistence.Repositories
{
    public class GenericRepository<TEntity> : IGenericRepository<TEntity> where TEntity : BaseEntity
    {
        private readonly CoreDbContext _dbContext;
        protected readonly DbSet<TEntity> _dbSet;

        public GenericRepository(CoreDbContext context)
        {
            _dbContext = context;
            _dbSet = context.Set<TEntity>();
        }
        
        public async Task<IList<TEntity>> Get(int pageSize = 10, int pageNumber = 1)
        {
            return await _dbSet
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
        }

        public virtual async Task<TEntity> GetById(int id)
        {
            return await _dbSet.AsNoTracking().FirstOrDefaultAsync(t => t.Id == id);
        }

        public virtual async Task<IEnumerable<TEntity>> GetByIds(int[] ids, params Expression<Func<TEntity, object>>[] includes)
        {
            return await includes
                     .Aggregate(
                         _dbSet.AsQueryable(),
                         (current, include) => current.Include(include)
                     )
                     .AsNoTracking()
                     .Where(e => ids.Contains(e.Id)).ToListAsync();
        }

        public async Task<TEntity> GetById(int id, params Expression<Func<TEntity, object>>[] includes) 
        {
                return await includes
                    .Aggregate(
                        _dbSet.AsQueryable(),
                        (current, include) => current.Include(include)
                    )
                    .AsNoTracking()
                    .FirstOrDefaultAsync(e => e.Id == id);
        }

        public async Task<IList<TEntity>> GetAll()
        {
            return await _dbSet
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<IList<TEntity>> GetAll(params Expression<Func<TEntity, object>>[] includes)
        {
            return await includes
                    .Aggregate(
                        _dbSet.AsQueryable(),
                        (current, include) => current.Include(include)
                    )
                    .AsNoTracking().ToListAsync();
        }

        public virtual async Task<int> Insert(TEntity record)
        {
            await _dbSet.AddAsync(record);
            await _dbContext.SaveChangesAsync();
            return record.Id;
        }

        public virtual async Task Update(TEntity record)
        {
            _dbSet.Attach(record);
            _dbContext.Entry(record).State = EntityState.Modified;
            await _dbContext.SaveChangesAsync();
        }

        public virtual async Task Delete(int id)
        {
            var result = _dbSet.Find(id);
            // Hard delete
            _dbSet.Remove(result);
            await _dbContext.SaveChangesAsync();
        }

        public async Task Disable(int id)
        {
            var result = await _dbSet.FindAsync(id);
            result.Disabled = true;
            await _dbContext.SaveChangesAsync();
        }

        public async Task Enable(int id)
        {
            var result = await _dbSet.FindAsync(id);
            result.Disabled = false;
            await _dbContext.SaveChangesAsync();
        }

        public async Task Hide(int id)
        {
            var result = await _dbSet.FindAsync(id);
            result.Removed = true;
            await _dbContext.SaveChangesAsync();
        }

        public async Task UnHide(int id)
        {
            var result = await _dbSet.FindAsync(id);
            result.Removed = false;
            await _dbContext.SaveChangesAsync();
        }

        public virtual async Task Delete(TEntity record)
        {
            record.Removed = true;
            _dbSet.Attach(record);
            _dbContext.Entry(record).State = EntityState.Modified;
            await _dbContext.SaveChangesAsync();
        }
    }
}
