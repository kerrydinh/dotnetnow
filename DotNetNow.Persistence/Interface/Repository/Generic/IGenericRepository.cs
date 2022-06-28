using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace DotNetNow.Persistence.Interface
{
    public interface IGenericRepository<TEntity>
    {
        Task<IList<TEntity>> Get(int pageSize = 10, int pageNumber = 1);
        Task<TEntity> GetById(int id);
        Task<TEntity> GetById(int id, params Expression<Func<TEntity, object>>[] includes);
        Task<IEnumerable<TEntity>> GetByIds(int[] ids, params Expression<Func<TEntity, object>>[] includes);
        Task<IList<TEntity>> GetAll();
        Task<IList<TEntity>> GetAll(params Expression<Func<TEntity, object>>[] includes);
        Task<int> Insert(TEntity record);
        Task Update(TEntity record);
        Task Delete(int id);
        Task Disable(int id);
        Task Enable(int id);
        Task Hide(int id);
        Task UnHide(int id);
        Task Delete(TEntity record);
    }
}