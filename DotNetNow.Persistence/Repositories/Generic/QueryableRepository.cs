using System.Collections.Generic;
using System.Threading.Tasks;
using DotNetNow.Domain.Base;
using DotNetNow.Domain.Entity;
using DotNetNow.Persistence.Interface;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace DotNetNow.Persistence.Repositories
{
    public class QueryableRepository<T> : GenericRepository<T>, IQueryableRepository<T> where T: QueryableEntity
    {
        public QueryableRepository(CoreDbContext context) : base(context)
        {
            
        }

        public async Task<IList<T>> Get(string name, int pageSize = 10, int pageNumber = 1)
        {
            return await _dbSet
                .Where(item => string.IsNullOrEmpty(name) || item.Name.ToLower().Contains(name.ToLower()))
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
        }
    }
}